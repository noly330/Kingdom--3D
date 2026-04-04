using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 10f;
    public float GetScanRadius() => _scanRadius;
    [SerializeField] private LayerMask _targetLayer;

    [Header("无需拖拽")]
    public NavMeshAgent navMeshAgent;
    public Transform target;
    public ScannerMode scannerMode;
    private TargetScanner _targetScanner = new TargetScanner();
    private float scannerTime = 0.3f;
    private float scannerTimer = 0f;

    private void Awake()
    {
        
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(scannerTimer <= 0f)
        {
            scannerTimer = scannerTime;
            switch(scannerMode)
            {
                case ScannerMode.Forward:
                    target = _targetScanner.FindForwardTarget(transform, _scanRadius, _targetLayer);
                    break;
                case ScannerMode.Nearest:
                    target = _targetScanner.FindNearestTarget(transform, _scanRadius, _targetLayer);
                    break;
            }
        }
        else
        {
            scannerTimer -= Time.deltaTime;
        }
    }

    public float GetDistanceToTarget()
    {
        if(target == null)
        {
            return float.MaxValue;
        }
        return Vector3.Distance(transform.position, target.position);
    }
}


