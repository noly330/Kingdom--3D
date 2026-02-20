using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyFSM : CombatControllerBase
{
    [SerializeField] private ComboListSO normalComboList;
    private IEnemyState currentState;
    private Dictionary<StateType,IEnemyState> states = new Dictionary<StateType, IEnemyState>();
    private NavMeshAgent agent;

    public NavMeshAgent M_agent => agent;

    [Header("巡逻范围和索敌")]
    public float patrolRange = 8f;

    public float viewDistance = 8f;
    public float viewAngle = 160f;
    public LayerMask obstacleMask;  //遮挡层，实现视野被墙等物品挡住

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        
    }

    protected override void Start()
    {
        base.Start();
        states.Add(StateType.Idle,new EnemyIdleState(this));
        states.Add(StateType.Patrol,new EnemyPatrolState(this));
        states.Add(StateType.Chase,new EnemyChaseState(this));
        states.Add(StateType.Fight,new EnemyFightState(this));
        states.Add(StateType.Dead,new EnemyDeadState(this));

        currentComboList = normalComboList;
        TransitionState(StateType.Idle);
    }

    protected override void Update()
    {
        base.Update();
        currentState.OnUpdate();
        CheckState();
    }

    public void TransitionState(StateType type)
    {
        if(currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    private void CheckState()
    {
        if(currentCharacter.currentHealth <= 0f && !currentCharacter.isDead)
        {
            animator.SetBool("IsDead",true);
            currentCharacter.isDead = true;
            TransitionState(StateType.Dead);
        }
    }

    protected override void CharacterCombatBeHit(ComboInteractionConfig interactionConfig, CharacterBase attacker)
    {
        base.CharacterCombatBeHit(interactionConfig, attacker);

        //看向攻击者
        transform.forward = -attacker.transform.forward;
        //播放受击动画
        animator.Play(interactionConfig.hitName, 0, 0);
        //生成受击特效
        var fxObj = hitFXList[(int)interactionConfig.attackForce].TryGetOneFXObj();
        ToolManager.instance.PlayOneFX(fxObj, hitPoints[0].position, Vector3.zero, new Vector3(1, 1, 1));
        //生成音效
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,patrolRange);
    }

}


