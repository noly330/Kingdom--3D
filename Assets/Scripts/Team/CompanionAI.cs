using UnityEngine;
using UnityEngine.AI;

public class CompanionAI : MonoBehaviour
{
    private TargetScanner _targetScanner = new TargetScanner();
    public Transform playerTransform;
    public Transform enemyTransform;
    public NavMeshAgent navMeshAgent;
    private CharacterController _characterController;
    [SerializeField] private LayerMask _EnemyLayer ;
    [SerializeField] private float _scanRadius = 10f;
    private float _scannerTime = 0.3f;
    private float _scannerTimer = 0f;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        EventCenter.Addlistener<Events.SwitchMainCharacter>(FindPlayerTransform);
        FindPlayerTransform(null);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.SwitchMainCharacter>(FindPlayerTransform);
    }

    private void Update()
    {
        if(_scannerTimer <= 0)
        {
            enemyTransform = _targetScanner.FindNearestTarget(transform, _scanRadius, _EnemyLayer);
        }
        // if(navMeshAgent != null && navMeshAgent.enabled)
        // {
        //     navMeshAgent.nextPosition = _characterController.transform.position;
        // }
        _scannerTimer -= Time.deltaTime;
    }

    public void FindPlayerTransform(Events.SwitchMainCharacter message)
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public float GetDistanceToPlayer()
    {
        if (playerTransform == null)
        {
            return 5201314;
        }
        return Vector3.Distance(transform.position, playerTransform.position);
    }

    public float GetDistanceToEnemy()
    {
        if (_targetScanner == null)
        {
            return 5201314;
        }
        return Vector3.Distance(transform.position, enemyTransform.position);
    }
}
