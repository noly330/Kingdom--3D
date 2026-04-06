using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScanner
{
    public Transform FindNearestTarget(Transform transform, float scanRadius, LayerMask targetLayer)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, scanRadius, targetLayer);
        if (targets.Length == 0)
            return null;

        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach(Collider collider in targets)
        {
            if(Vector3.Distance(transform.position, collider.transform.position) < nearestDistance)
            {
                nearestTarget = collider.transform;
                nearestDistance = Vector3.Distance(transform.position, collider.transform.position);
            }
        }
        return nearestTarget;
    }

    public Transform FindForwardTarget(Transform transform, float scanRadius, LayerMask targetLayer)
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
