using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionMovementAgent : CharacterMovementControlBase
{
    private bool _hasInput;

    protected override void Update()
    {
        base.Update();
        SetAnimatorValue();
    }
    

    public void SetHasInput(bool hasInput)
    {
        _hasInput = hasInput;
    }


    private void SetAnimatorValue()
    {
        _animator.SetFloat("VerticalVelocity", _verticalVelocity);
        _animator.SetBool("IsGround", isGround);
    }

    public void SetAnimatorMovementValue(float movement)
    {
        _animator.SetFloat("Movement", movement, 0.2f, Time.deltaTime);
    }

    public void PlayFootSound()
    {

    }
}
