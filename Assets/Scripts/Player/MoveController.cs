using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveController : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerInput playerInput;
    private GameObject mainCamera;
    private Vector2 move;
    private float targetRot = 0.0f;

    //平滑旋转时间
    private float rotationSmoothTime = 0.1f;
    private float axisY;
    private float rotationVelocity;
    private Animator animator;

    private bool isRun;

    // 地面检测相关
    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer = -1; // 默认所有层
    private RaycastHit groundHit;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGround = CheckGround();

        AnimationManager();
        ColdTime();
        CaculateGravity();
        GroundMove();
    }

    // 优化的地面检测方法
    private bool CheckGround()
    {

        float checkDistance = characterController.height / 2 + groundCheckDistance;
        Vector3 rayOrigin = transform.position + characterController.center;  //获得胶囊体中心(角色世界坐标加上胶囊体中心偏移量)

        if (Physics.Raycast(rayOrigin, Vector3.down, out groundHit, checkDistance, groundLayer))
        {
            //groundHit.normal：地面的法向量
            float slopeAngle = Vector3.Angle(groundHit.normal, Vector3.up);  //计算当前地面的
            return slopeAngle <= characterController.slopeLimit;
        }

        return false;

    }

    void AnimationManager()
    {
        animator.SetFloat("AxisY", axisY);
        animator.SetBool("IsRun", isRun);
        animator.SetBool("IsGround", isGround);
    }

    void GroundMove()
    {
        if (!isGround) return;
        Vector3 inputDir = new Vector3(move.x, 0.0f, move.y).normalized;
        //Mathf.Atan2(inputDir.x, inputDir.z)：计算输入方向的弧度值
        //* Mathf.Rad2Deg：把弧度转换成角度
        targetRot = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref rotationVelocity, rotationSmoothTime);
        if (move != Vector2.zero)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        isRun = playerInput.actions["Run"].IsPressed() && move.magnitude > 0.1f;

        axisY = animator.GetFloat("AxisY");
        axisY = Mathf.MoveTowards(axisY, move.magnitude, 5f * Time.deltaTime);
    }

    #region 触发按键
    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    private void OnSlide()
    {
        if (slideColdTime <= 0.1f)
        {
            animator.CrossFadeInFixedTime("Slide", 0, 0, 0);
            slideColdTime = 1f;
        }
    }

    #endregion

    #region 冷却相关

    private float slideColdTime = 0f;
    private void ColdTime()
    {
        if (slideColdTime > 0.1f)
        {
            slideColdTime -= Time.deltaTime;
        }
    }

    #endregion

    #region 跳跃相关
    public float jumpSpeed = 1f;
    private float gravity = -9.8f;
    private Vector3 velocity = Vector3.zero;
    public bool isGround;

    public float deceleration = 5f; // 减速速度
    private void OnJump()
    {
        if (!isGround || animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt")) return;

        velocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
        animator.Play("Jump", 0, 0);
    }

    private void CaculateGravity()
    {
        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;

            velocity.x = 0f;
            velocity.z = 0f;
        }
        else
        {
            Vector3 inputDir = new Vector3(move.x, 0.0f, move.y).normalized;
            targetRot = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            if (move != Vector2.zero)
            {
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRot, ref rotationVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
                Vector3 targetDir = Quaternion.Euler(0f, targetRot, 0f) * Vector3.forward;
                Vector3 v = isRun ? targetDir.normalized * 1.5f * jumpSpeed : targetDir.normalized * 0.7f * jumpSpeed;
                velocity.x = v.x;
                velocity.z = v.z;
            }
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
    #endregion
}