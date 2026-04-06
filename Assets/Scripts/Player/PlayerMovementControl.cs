using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementControl : CharacterMovementControlBase
{
    private float _angleVelocity;
    private GameObject _mainCamera;
    [SerializeField] private float _rotationSmoothTime;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 1.6f;
    [SerializeField] private float _jumpCooldown = 0.1f;
    [SerializeField] private float _airSpeed = 5f;
    private float _jumpCooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = GameObject.Find("Main Camera");
    }

    protected override void Update()
    {
        base.Update();
        //CharacterRotationControl();
        RotationController();
        CharacterSlide();
        HandleJump();
        UpdateJumpCooldown();
        UpdateAniamator();
    }

    private void LateUpdate()
    {
    }

    protected override void OnAnimatorMove()
    {
        _animator.ApplyBuiltinRootMotion();
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump") || !isGround)
        {
            Vector3 inputDir = new Vector3(GameInputManager.Instance.Move.x, 0.0f, GameInputManager.Instance.Move.y);
            Vector3 targetDir = Quaternion.Euler(0f, _mainCamera.transform.eulerAngles.y, 0f) * inputDir;

            UpdateCharacterMoveDirection(targetDir * _airSpeed);
        }
        else
        {
            UpdateCharacterMoveDirection(_animator.deltaPosition);
        }
    }

    public float _targetRot = 0.0f;
    public Vector3 _targetDirection;
    Vector3 _inputDir;

    private void RotationController()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Turn")) return;
            
        _inputDir = new Vector3(GameInputManager.Instance.Move.x, 0.0f, GameInputManager.Instance.Move.y).normalized;
        //Mathf.Atan2(inputDir.x, inputDir.z)：计算输入方向的弧度值
        //* Mathf.Rad2Deg：把弧度转换成角度
        _targetRot = Mathf.Atan2(_inputDir.x, _inputDir.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        _targetDirection = Quaternion.Euler(0f, _targetRot, 0f) * Vector3.forward;

        float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRot, ref _angleVelocity, _rotationSmoothTime);
        if (GameInputManager.Instance.Move != Vector2.zero)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt"))
                transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }
        _animator.SetFloat(AnimationID.DetalAngleID, Vector3.SignedAngle(transform.forward, _targetDirection, Vector3.up));
    }

    private void CharacterSlide()
    {
        if (GameInputManager.Instance.Slide)
        {

            transform.eulerAngles = Vector3.up * _targetRot;
            _animator.CrossFadeInFixedTime("Slide", 0, 0, 0);
        }
    }

    #region 跳跃

    private void HandleJump()
    {
        if (_jumpCooldownTimer > 0f) return;
        if (!isGround) return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Turn")) return;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Slide") && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f) return;

        if (GameInputManager.Instance.Jump)
        {

            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _jumpCooldownTimer = _jumpCooldown;

            _animator.SetTrigger(AnimationID.JumpID);
        }
    }
    private void UpdateJumpCooldown()
    {
        if (_jumpCooldownTimer > 0f)
        {
            _jumpCooldownTimer -= Time.deltaTime;
        }
    }

    #endregion

    private void UpdateAniamator()
    {
        _animator.SetFloat(AnimationID.VerticalVelocityID, _verticalVelocity);
        _animator.SetBool(AnimationID.IsGroundID, isGround);
        _animator.SetBool(AnimationID.IsFallID, isFall);
        //if (!isGround) return;

        _animator.SetBool(AnimationID.HasInputID, GameInputManager.Instance.Move != Vector2.zero);

        if (_animator.GetBool(AnimationID.HasInputID))
        {
            _animator.SetFloat(AnimationID.MovementID, (_animator.GetBool(AnimationID.IsRunID) ? 3f : 2f * GameInputManager.Instance.Move.magnitude), 0.1F, Time.deltaTime);

            if (GameInputManager.Instance.Run)
                _animator.SetBool(AnimationID.IsRunID, true);
        }
        else
        {
            _animator.SetFloat(AnimationID.MovementID, 0f, 0.3f, Time.deltaTime);
            if (_animator.GetFloat(AnimationID.MovementID) < 0.2f)
            {
                _animator.SetBool(AnimationID.IsRunID, false);
            }
        }
    }
}