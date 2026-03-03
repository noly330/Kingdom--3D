using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyFSM : CombatControllerBase
{
    [SerializeField] private ComboListSO normalComboList;
    private IEnemyState currentState;
    private Dictionary<StateType, IEnemyState> states = new Dictionary<StateType, IEnemyState>();
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
        states.Add(StateType.Idle, new EnemyIdleState(this));
        states.Add(StateType.Patrol, new EnemyPatrolState(this));
        states.Add(StateType.Chase, new EnemyChaseState(this));
        states.Add(StateType.Fight, new EnemyFightState(this));
        states.Add(StateType.Dead, new EnemyDeadState(this));

    }

    private void OnEnable()
    {
        TransitionState(StateType.Idle);

    }

    protected override void Start()
    {
        base.Start();

        currentComboList = normalComboList;
    }



    protected override void Update()
    {
        base.Update();
        currentState.OnUpdate();
    }

    public void TransitionState(StateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }
    public void TransitionToDeadState()
    {
        TransitionState(StateType.Dead);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }


}


