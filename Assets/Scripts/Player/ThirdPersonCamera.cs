using System.Collections;
using System.Collections.Generic;
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

    private const float threshold = 0.01f;  //输入阈值，过滤无效的微小输入
    private float cinemachineTargetYaw;  //记录相机 / 角色的水平旋转角度（绕 Y 轴）
    private float cinemachineTargetPitch;  //记录相机的垂直旋转角度（绕 X 轴）
    private Vector2 look;  //存储鼠标视角输入
    private Vector2 scroll; //存储鼠标滚轮输入
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
    }


    private void Update()
    {
        if (look.sqrMagnitude >= threshold)
        {
            cinemachineTargetYaw += look.x * 0.5f;
            cinemachineTargetPitch += look.y * 0.5f;
        }

        //把角度归一化到正确的区间
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        //旋转相机
        cameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0.0f);

        HandleFovZoom();
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

    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();

    }

    public void OnScroll(InputValue value)
    {
        scroll = value.Get<Vector2>();
    }
}
