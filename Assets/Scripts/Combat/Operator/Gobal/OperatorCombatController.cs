using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OperatorCombatController : CombatControllerBase
{
    [SerializeField] private CombatListSO[] _skillCombatLists;
    public CombatListSO GetSkillComabtList(int index) => _skillCombatLists[index];
    [SerializeField] private CombatListSO[] _linkCombatLists;
    public CombatListSO GetLinkCombatList(int index) => _linkCombatLists[index];
    [SerializeField] private CombatListSO _ultimateCombatList;
    [SerializeField] private float _skillDistance = 2.5f;
    public float skillDistance => _skillDistance;
    [SerializeField] private float _linkDistance = 2.5f;
    public float linkDistance => _linkDistance;
    public CombatListSO ultimateCombatList => _ultimateCombatList;

    public float linkEnergy = 15f;
    public float currentLinkEnergy = 15f;


    public bool CanSkillAttack()
    {
        return TeamManager.Instance.teamCurrentEnergy >= 100;
    }

    protected void LinkSkillEnemyRecover()
    {
        if (currentLinkEnergy < linkEnergy + 0.01f)
        {
            currentLinkEnergy += Time.deltaTime * 1f;
        }
        else
        {
            currentLinkEnergy = linkEnergy;
        }
    }

    public override void BeHit(CombatInteractionConfig interactionConfig, CharacterBase attacker)
    {
        if (_combatStateMachine.GetCurrentStateType() == CombatStateType.Avoid)
        {
            return;
        }
        // 完美闪避触发条件只放在受击入口：角色处于Slide动画标签时被命中，改为进入Avoid状态，不结算本次受击。
        if (_combatStateMachine.GetCurrentStateType() == CombatStateType.Slide)
        {
            _combatStateMachine.ForceTransitionTo(CombatStateType.Avoid);
            return;
        }

        base.BeHit(interactionConfig, attacker);
    }

    /// <summary>
    /// 如果是非主控干员，就把该干员瞬移到缓存目标位置
    /// </summary>
    /// <returns></returns>
    public bool TeleportNearCachedAttackTargetIfCompanion(float distance)
    {
        if (TeamManager.Instance == null) return false;
        int slotIndex = TeamManager.Instance.GetSlotIndex(transform);
        if (slotIndex < 0 || slotIndex == TeamManager.Instance.mainCharacterIndex) return false;
        if (!TryGetCachedAttackTarget(out Transform target)) return false;

        float sideSign = slotIndex % 2 == 0 ? -1f : 1f;
        Vector3 side = target.right.sqrMagnitude > 0.01f ? target.right : Vector3.right;
        Vector3 targetPosition = target.position + side.normalized * sideSign * distance;

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, distance, NavMesh.AllAreas))  //在计算出的位置附近distance范围内查找最近的导航网格有效位置，确保角色传送后站在可行走区域
            targetPosition = hit.position;

        CharacterController characterController = GetComponent<CharacterController>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        bool wasCharacterControllerEnabled = characterController != null && characterController.enabled;

        if (characterController != null)
            characterController.enabled = false;

        if (agent != null && agent.enabled)  //如果有NavMeshAgent且启用，使用Warp方法（告诉导航系统这是合法传送）
            agent.Warp(targetPosition);
        else
            transform.position = targetPosition;  //否则直接设置位置

        if (characterController != null)
            characterController.enabled = wasCharacterControllerEnabled;

        Vector3 lookDirection = target.position - transform.position;
        lookDirection.y = 0f;
        if (lookDirection.sqrMagnitude > 0.01f)
            transform.forward = lookDirection.normalized;

        _attackTarget = target;
        return true;
    }

    private static Coroutine _linkSkillSlowCoroutine;
    private static float _linkSlowEndTime;
    private static float _savedTimeScale = 1f;
    private static float _savedFixedDeltaTime = 0.02f;
    private static bool _isLinkSlowActive;

    /// <summary>
    /// 连携技全局时间减速 (0.3x, 持续 1s) + 非主控干员镜头偏移。连续释放时后者覆盖前者。
    /// </summary>
    public void StartLinkSkillTimeSlow()
    {
        _linkSlowEndTime = Time.realtimeSinceStartup + 0.5f;

        bool isCompanion = TeamManager.Instance != null &&
                           TeamManager.Instance.GetSlotIndex(transform) != TeamManager.Instance.mainCharacterIndex;

        if (isCompanion)
        {
            ThirdPersonCamera camera = FindObjectOfType<ThirdPersonCamera>();
            camera?.StartLookAt(transform);
        }

        if (_linkSkillSlowCoroutine == null)
            _linkSkillSlowCoroutine = StartCoroutine(LinkSkillSlowCoroutine());
    }

    private IEnumerator LinkSkillSlowCoroutine()
    {
        if (!_isLinkSlowActive)
        {
            _savedTimeScale = Time.timeScale;
            _savedFixedDeltaTime = Time.fixedDeltaTime;
        }
        _isLinkSlowActive = true;

        Time.timeScale = 0.3f;
        Time.fixedDeltaTime = _savedFixedDeltaTime * 0.3f;

        while (Time.realtimeSinceStartup < _linkSlowEndTime)
            yield return null;

        Time.timeScale = _savedTimeScale;
        Time.fixedDeltaTime = _savedFixedDeltaTime;
        _isLinkSlowActive = false;

        // 减速结束后平滑复原镜头
        ThirdPersonCamera camera = FindObjectOfType<ThirdPersonCamera>();
        camera?.StartReturn();

        _linkSkillSlowCoroutine = null;
    }

    
}
