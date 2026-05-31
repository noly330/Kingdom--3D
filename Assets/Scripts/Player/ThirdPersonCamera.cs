using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCamera : MonoBehaviour
{
    private GameObject mainCamera;
    [Header("Cinemachine设置")]
    public GameObject cameraTarget;
    public float topClamp = 70f;
    public float bottomClamp = -30f;

    [Header("FOV缩放设置")]
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public float fovMin = 30f;
    public float fovMax = 70f;
    public float fovSensitivity = 10f;  //缩放灵敏度
    private float targetFov;

    [Header("公开的第三人称相机设置")]
    public float mouseSensitivity = 0.3f;
    private const float threshold = 0.01f;  //输入阈值，过滤无效的微小输入
    private float cinemachineTargetYaw;  //记录相机 / 角色的水平旋转角度（绕 Y 轴）
    private float cinemachineTargetPitch;  //记录相机的垂直旋转角度（绕 X 轴）
    private Vector2 look;  //存储鼠标视角输入
    private Vector2 scroll; //存储鼠标滚轮输入

    // 镜头注视覆盖（用于连携技镜头偏移）
    private bool _isLookAtOverride;
    private float _lookAtTargetYaw;
    private float _lookAtTargetPitch;
    private float _lookAtOriginalYaw;
    private float _lookAtOriginalPitch;
    private bool _isReturning;
    private float _returnDuration = 0.5f;
    private float _returnTimer;

    private void OnEnable()
    {
        EventCenter.AddListener<Events.SwitchMainCharacter>(ChangeCameraTarget);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.SwitchMainCharacter>(ChangeCameraTarget);
    }

    private void ChangeCameraTarget(Events.SwitchMainCharacter message)
    {
        Transform newTarget = TeamManager.Instance.teamMembers[message.NewIndex];
        Transform newCameraPoint = null;
        foreach (Transform child in newTarget.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag("CameraPoint"))
            {
                newCameraPoint = child;
                break;
            }
        }
        if (newCameraPoint == null)
        {
            Debug.LogError("新目标没有没有相机点,请检查是否有Tag为CameraPoint的子物体");
            return;
        }
        cameraTarget = newCameraPoint.gameObject;
        virtualCamera.Follow = newCameraPoint;
    }

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        cinemachineTargetYaw = cameraTarget.transform.rotation.eulerAngles.y;  //保留摄像机y角度

        if (virtualCamera != null)
        {
            targetFov = virtualCamera.m_Lens.FieldOfView;
        }

        // Cursor.visible = false;  //游标不可见
        // Cursor.lockState = CursorLockMode.Locked;  //游标的锁定模式为锁定
    }


    private void LateUpdate()
    {
        if (_isLookAtOverride)
        {
            float delta = Time.unscaledDeltaTime;

            if (!_isReturning)
            {
                cinemachineTargetYaw = Mathf.LerpAngle(cinemachineTargetYaw, _lookAtTargetYaw, delta * 6f);
                cinemachineTargetPitch = Mathf.LerpAngle(cinemachineTargetPitch, _lookAtTargetPitch, delta * 6f);
            }
            else
            {
                _returnTimer -= delta;
                cinemachineTargetYaw = Mathf.LerpAngle(cinemachineTargetYaw, _lookAtOriginalYaw, delta * 3f);
                cinemachineTargetPitch = Mathf.LerpAngle(cinemachineTargetPitch, _lookAtOriginalPitch, delta * 3f);

                if (_returnTimer <= 0f)
                    _isLookAtOverride = false;
            }
        }
        else
        {
            look = GameInputManager.Instance.Look;
            scroll = GameInputManager.Instance.Scroll;
            if (look.sqrMagnitude >= threshold)
            {
                cinemachineTargetYaw += look.x * mouseSensitivity;
                cinemachineTargetPitch += look.y * mouseSensitivity * 0.4f;
            }
        }

        //把角度归一化到正确的区间
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        //旋转相机
        cameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);

        HandleFovZoom();
    }

    /// <summary>
    /// 连携技镜头偏移：平滑旋转镜头注视目标干员
    /// </summary>
    public void StartLookAt(Transform target)
    {
        if (target == null || virtualCamera == null) return;

        _isLookAtOverride = true;
        _isReturning = false;

        _lookAtOriginalYaw = cinemachineTargetYaw;
        _lookAtOriginalPitch = cinemachineTargetPitch;

        Vector3 lookDir = target.position - virtualCamera.transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude < 0.01f) return;

        Quaternion lookRotation = Quaternion.LookRotation(lookDir);
        _lookAtTargetYaw = lookRotation.eulerAngles.y;
        _lookAtTargetPitch = cinemachineTargetPitch;
    }

    /// <summary>
    /// 连携技镜头复原：平滑回到原注视方向
    /// </summary>
    public void StartReturn()
    {
        _isReturning = true;
        _returnTimer = _returnDuration;
    }

    private void HandleFovZoom()
    {
        if (virtualCamera == null) return;

        // 应用滚轮输入改变目标FOV
        if (scroll.y != 0)
        {
            targetFov -= scroll.y * fovSensitivity * Time.deltaTime;
            targetFov = Mathf.Clamp(targetFov, fovMin, fovMax);
        }

        // 平滑过渡到目标FOV
        float currentFov = virtualCamera.m_Lens.FieldOfView;
        if (Mathf.Abs(currentFov - targetFov) > threshold)
        {
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(currentFov, targetFov, Time.deltaTime * 7f);
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);//Mathf.Clamp限制角度在lfMin~lfMax范围内
    }

    // public void OnLook(InputValue value)
    // {
    //     look = value.Get<Vector2>();

    // }

    // public void OnScroll(InputValue value)
    // {
    //     scroll = value.Get<Vector2>();
    // }
}
