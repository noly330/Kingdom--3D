using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScanner : MonoBehaviour
{
    public Transform FindNearestTarget(float scanRadius, LayerMask targetLayer)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, scanRadius, targetLayer);
        if (targets.Length == 0)
            return null;
        return targets[0].transform;
    }

    public Transform FindForwardTarget(float scanRadius, LayerMask targetLayer)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, scanRadius, targetLayer);

        foreach (var collider in targets)
        {
            Vector3 targetDir = collider.transform.position - transform.position;
            targetDir.y=0;
            float angle = Vector3.Angle(transform.forward, targetDir);
            if (angle < 90f)
            {
                return collider.transform;
            }
        }
        return null;
    }


    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(transform.position, _scanRadius);
    // }
}
