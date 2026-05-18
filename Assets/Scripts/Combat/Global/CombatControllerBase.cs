using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatControllerBase : MonoBehaviour, IDamageable
{
    [SerializeField] private CombatListSO _normalCombatList;
    public CombatListSO normalCombatList => _normalCombatList;
    public ForceLevel poiseLevel;
    public Animator animator;
    public LayerMask targetMask;
    protected CharacterBase _characterBase;
    public CharacterBase characterBase => _characterBase;
    protected CombatStateMachineBase _combatStateMachine;
    private float _normalAttackCooldownTimer;
    private float _resetNormalAttackIndexTimer;
    [Header("索敌设置")]
    protected Transform _attackTarget;
    [SerializeField] private Vector3 checkSize = new Vector3(3, 3, 3);

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        _characterBase = GetComponent<CharacterBase>();
        _combatStateMachine = GetComponent<CombatStateMachineBase>();
    }

    protected virtual void Update()
    {
        ColdTimer();
    }
    #region 普攻冷却
    public bool CanNormalAttack()
    {
        return _normalAttackCooldownTimer < 0.01f;
    }

    public void TriggerNormalAttackCold(float coldTime)
    {
        _normalAttackCooldownTimer = coldTime;
    }

    public void TriggerResetNormalAttackIndexCold(float coldTime)
    {
        _resetNormalAttackIndexTimer = 3.5f * coldTime;
    }

    public bool ResetNormalAttackIndex()
    {
        return _resetNormalAttackIndexTimer < 0.01f;
    }

    protected virtual void ColdTimer()
    {
        if (_normalAttackCooldownTimer > 0)
            _normalAttackCooldownTimer -= Time.deltaTime;
        if (_resetNormalAttackIndexTimer > 0)
            _resetNormalAttackIndexTimer -= Time.deltaTime;
    }
    #endregion

    #region 索敌
    public void FindTarget()
    {
        Collider[] targetList = Physics.OverlapBox(transform.position, checkSize, Quaternion.identity, targetMask);
        if (targetList.Length == 0)
        {
            _attackTarget = null;
            return;
        }

        //找最近目标
        float distanceMin = float.MaxValue;
        foreach (var target in targetList)
        {
            float dis = Vector3.Distance(target.transform.position, transform.position);
            if (dis < distanceMin)
            {
                distanceMin = dis;
                _attackTarget = target.transform;
            }
        }
    }
    public void LookTarget()
    {
        if (_attackTarget == null) return;
        Vector3 dir = _attackTarget.position - transform.position;
        dir.y = 0;
        transform.forward = dir.normalized;
    }
    #endregion

    #region 攻击效果

    public void PlayVFX(CombatVFXConfig vfxConfig)
    {
        if (vfxConfig?.VFXObject == null) return;

        Vector3 vfxPosition = transform.position
                            + transform.right * vfxConfig.position.x
                            + transform.up * vfxConfig.position.y
                            + transform.forward * vfxConfig.position.z;

        Quaternion rotation = transform.rotation * Quaternion.Euler(vfxConfig.rotation);

        GameObject fx = Instantiate(vfxConfig.VFXObject, vfxPosition, rotation);
        fx.transform.localScale = vfxConfig.scale;
    }

    public void PlaySFX(CombatSFXConfig SfxConfig)
    {
        if (SfxConfig?.audioClip == null) return;

        AudioManager.instance.PlaySFX(SfxConfig.audioClip, SfxConfig.volume);
    }

    #endregion

    public virtual void BeHit(CombatInteractionConfig interactionConfig, CharacterBase attacker)
    {
        if (interactionConfig == null) return;
        if (_characterBase.isInvulnerable) return;

        Damage newDamage = attacker.TryGetDamage(interactionConfig);
        _characterBase.BeHit(newDamage);

        //看向攻击者
        //播放受击动画

        if (interactionConfig.attackForceLevel < poiseLevel)
            return;
        transform.forward = -attacker.transform.forward;

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Hit_Up"))
            animator.Play(interactionConfig.hitName, 0, 0);
            
        _combatStateMachine.TryTransitionTO(CombatStateType.Hit);
    }

}

public class RunningEventIndex
{
    public int attackDetectionIndex = 0;
    public int VFXIndex = 0;
    public int SFXIndex = 0;
    public int AttackFeedbackIndex = 0;
    public void Reset()
    {
        attackDetectionIndex = 0;
        VFXIndex = 0;
        SFXIndex = 0;
        AttackFeedbackIndex = 0;
    }
}