using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementControl : CharacterMovementControlBase
{
    private float _rotationAngle;
    private float _angleVelocity = 0f;
    private GameObject _mainCamera;
    [SerializeField] private float _rotationSmoothTime;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = GameObject.Find("Main Camera");
    }

    protected override void Update()
    {
        base.Update();
        CharacterRotationControl();
        CharacterSlide();
        UpdateAniamator();
    }

    private void LateUpdate()
    {
    }

    private void CharacterRotationControl()
    {
        if (!isGround) return;

        if (_animator.GetBool("HasInput"))
            _rotationAngle = Mathf.Atan2(GameInputManager.Instance.Move.x, GameInputManager.Instance.Move.y) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;


        if (_animator.GetBool("HasInput") &&
        (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Motion") || _animator.GetCurrentAnimatorStateInfo(0).IsTag("Slide")))
        {
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, _rotationAngle, ref _angleVelocity, _rotationSmoothTime);
        }

    }

    private void CharacterSlide()
    {
        if (GameInputManager.Instance.playerInput.actions["Slide"].triggered)
        {
            
            transform.eulerAngles = Vector3.up * _rotationAngle;
            _animator.CrossFadeInFixedTime("Slide", 0, 0, 0);
        }
    }

    private void UpdateAniamator()
    {
        if (!isGround) return;

        _animator.SetBool("HasInput", GameInputManager.Instance.Move != Vector2.zero);

        if (_animator.GetBool("HasInput"))
        {
            _animator.SetFloat("Movement", (_animator.GetBool("IsRun") ? 2f : GameInputManager.Instance.Move.magnitude), 0.25f, Time.deltaTime);

            if (GameInputManager.Instance.Run)
                _animator.SetBool("IsRun", true);
        }
        else
        {
            _animator.SetFloat("Movement", 0f, 0.25f, Time.deltaTime);
            if (_animator.GetFloat("Movement") < 0.2f)
            {
                _animator.SetBool("IsRun", false);
            }
        }
    }
}
