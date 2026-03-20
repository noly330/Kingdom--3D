using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : CharacterMovementControlBase
{
    private bool _applyMovement;

    protected override void Start()
    {
        base.Start();
        SetApplyMovement(true);
    }

    protected override void Update()
    {
        base.Update();
        LockTargetDirection();
        DrawDirection();
    }


    private void LockTargetDirection()
    {
        transform.LookAt(EnemyManager.Instance.GetMainPlayer());
    }

    public void SetAnimatorMovementValue(float horizontal, float vertical)
    {
        if (_applyMovement)
        {
            _animator.SetFloat("Lock", 1f);
            _animator.SetFloat("Horizontal", horizontal, 0.2f, Time.deltaTime);
            _animator.SetFloat("Vertical", vertical, 0.2f, Time.deltaTime);
        }
        else
        {
            _animator.SetFloat("Lock", 0f);
            _animator.SetFloat("Horizontal", 0f, 0.2f, Time.deltaTime);
            _animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
        }
    }

    public void SetApplyMovement(bool apply)
    {
        _applyMovement = apply;
    }

    private void DrawDirection()
    {
        Debug.DrawRay(transform.position + (transform.up * 0.5f),
         EnemyManager.Instance.GetMainPlayer().position - transform.position, Color.yellow);
    }

    public void PlayFootSound()
    {
        
    }
}
