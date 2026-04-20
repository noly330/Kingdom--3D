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
        if (interactionConfig == null) return;
        if (_characterBase.isInvulnerable) return;

        Damage newDamage = attacker.TryGetDamage(interactionConfig);
        _characterBase.BeHit(newDamage);

        //看向攻击者
        transform.forward = -attacker.transform.forward;
        //播放受击动画
        if (interactionConfig.attackEffectType == AttackEffectType.None)
        {

            animator.Play(interactionConfig.hitName, 0, 0);
            _combatStateMachine.TryTransitionTO(CombatStateType.Hit);
        }
        else
        {
            PhysicsAbnormality(interactionConfig.attackEffectType, attacker);
        }
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
                if (_enemyAbnormalityManager.breakstack <= 4)
                    _enemyAbnormalityManager.breakstack++;
                break;
            case AttackEffectType.KnockDown:
                if (_enemyAbnormalityManager.breakstack <= 4)
                    _enemyAbnormalityManager.breakstack++;
                //TODO:播放倒地动画
                break;
            case AttackEffectType.Smash:
                if (_enemyAbnormalityManager.breakstack > 0)
                {
                    Damage newDamage = new Damage(attacker.currentAttack, false);
                    newDamage.damage *= _enemyAbnormalityManager.breakstack;
                    _characterBase.BeHit(newDamage);
                    _enemyAbnormalityManager.breakstack = 0;
                }
                else
                {
                    _enemyAbnormalityManager.breakstack++;
                }
                break;
            case AttackEffectType.Sunder:
            //TODO:碎甲效果
                break;

        }
    }



}


