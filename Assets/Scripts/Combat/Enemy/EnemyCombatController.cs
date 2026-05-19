using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : CombatControllerBase
{
    private EnemyAbnormalityManager _enemyAbnormalityManager;
    protected override void Awake()
    {
        base.Awake();
        _enemyAbnormalityManager = GetComponent<EnemyAbnormalityManager>();
    }
    public override void BeHit(CombatInteractionConfig interactionConfig, CharacterBase attacker)
    {
        //直接不要基类的实现了
        if (interactionConfig == null) return;
        if (_characterBase.isInvulnerable) return;

        //传递伤害
        Damage newDamage = attacker.TryGetDamage(interactionConfig);
        newDamage.damage *= _enemyAbnormalityManager.GetShatterDamageMultiplier();
        _characterBase.BeHit(newDamage);
        //看向攻击者
        transform.forward = -attacker.transform.forward;
        //播放受击动画
        if (ChangeHitAnimation())
            animator.CrossFadeInFixedTime(interactionConfig.hitName, 0.1f, 0);

        _combatStateMachine.TryTransitionTO(CombatStateType.Hit);
        if (interactionConfig.attackEffectType != AttackEffectType.None)
        {
            PhysicsAbnormality(interactionConfig.attackEffectType, attacker);
        }
    }


    private bool ChangeHitAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit_Up") || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit_Down"))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 处理物理异常
    /// </summary>
    /// <param name="attackEffectType"></param>
    /// <param name="attacker"></param>
    public void PhysicsAbnormality(AttackEffectType attackEffectType, CharacterBase attacker)
    {
        switch (attackEffectType)
        {
            case AttackEffectType.Launch:

                if (_enemyAbnormalityManager.breakStack > 0)
                    animator.CrossFadeInFixedTime("Hit_Up", 0.1f, 0);
                _enemyAbnormalityManager.OnPhysicalDefenseBreakApplied();
                break;
            case AttackEffectType.KnockDown:

                if (_enemyAbnormalityManager.breakStack > 0)
                {
                    animator.CrossFadeInFixedTime("Hit_Down", 0.1f, 0);
                }

                _enemyAbnormalityManager.OnPhysicalDefenseBreakApplied();

                break;
            case AttackEffectType.Smash:
                if (_enemyAbnormalityManager.breakStack > 0)
                {
                    Damage newDamage = new Damage(attacker.currentAttack, false);

                    //猛击的时候，会额外造成攻击力×破防层数的伤害
                    newDamage.damage *= _enemyAbnormalityManager.breakStack * 1.25f;  //消耗破防层数的额外增伤
                    //碎甲效果带来的乘区
                    newDamage.damage *= _enemyAbnormalityManager.GetShatterDamageMultiplier();

                    _characterBase.BeHit(newDamage);
                    _enemyAbnormalityManager.ResetBreakStack();
                }
                else
                {
                    _enemyAbnormalityManager.OnPhysicalDefenseBreakApplied();
                }
                break;
            case AttackEffectType.Sunder:

                if (_enemyAbnormalityManager.breakStack > 0)
                {
                    _enemyAbnormalityManager.TriggerShatter(_enemyAbnormalityManager.breakStack);
                    Damage newDamage = new Damage(attacker.currentAttack, false);
                    newDamage.damage *= 0.2f * _enemyAbnormalityManager.breakStack;  //消耗破防层数的额外增伤
                    newDamage.damage *= _enemyAbnormalityManager.GetShatterDamageMultiplier();
                    _characterBase.BeHit(newDamage);
                    _enemyAbnormalityManager.ResetBreakStack();
                }
                else
                {
                    _enemyAbnormalityManager.OnPhysicalDefenseBreakApplied();
                }
                break;

        }
    }



}


