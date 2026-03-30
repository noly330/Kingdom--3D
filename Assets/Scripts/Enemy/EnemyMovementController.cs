using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : CharacterMovementControlBase
{
    private EnemyAI _enemyAI;

    private bool _applyFightMovement;

    protected override void Awake()
    {
        base.Awake();
        _enemyAI = GetComponent<EnemyAI>();
    }

    protected override void Start()
    {
        base.Start();
        SetApplyFightMovement(true);
    }

    protected override void Update()
    {
        base.Update();
        //LockTargetDirection();
        DrawDirection();
    }

    public void SetAnimatorMovementValue(float movement)
    {
        if (_applyFightMovement)
            return;
        _animator.SetFloat("Lock", 0f, 0.3f, Time.deltaTime);
        _animator.SetFloat("Movement", movement, 0.2f, Time.deltaTime);
    }

    public void SetFightAnimatorMovementValue(float horizontal, float vertical)
    {
        if (!_applyFightMovement)
            return;

        _animator.SetFloat("Lock", 1f, 0.3f, Time.deltaTime);
        _animator.SetFloat("Horizontal", horizontal, 0.2f, Time.deltaTime);
        _animator.SetFloat("Vertical", vertical, 0.2f, Time.deltaTime);

    }

    public void SetApplyFightMovement(bool apply)
    {
        _applyFightMovement = apply;
    }

    private void DrawDirection()
    {
        if (_enemyAI.target == null)
            return;
        Debug.DrawRay(transform.position + (transform.up * 0.5f),
         _enemyAI.target.position - transform.position, Color.yellow);
    }

    public void PlayFootSound()
    {

    }
}
