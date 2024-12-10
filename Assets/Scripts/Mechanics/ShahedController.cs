using System.Collections.Generic;
using UnityEngine;

public class ShahedController : MonoBehaviour
{
    public float speed = 10f;
    public float diveSpeed = 20f;
    public float diveDistance = 15f;
    public float collisionThreshold = 1f;

    private Transform targetPoint;
    private bool isDiving = false;
    private static List<Transform> assignedTargets = new List<Transform>();

    void Update()
    {
        if (targetPoint != null)
        {
            Vector3 targetXZ = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);
            float distanceXZ = Vector3.Distance(transform.position, targetXZ);

            if (!isDiving && distanceXZ > diveDistance)
            {
                Vector3 direction = (targetXZ - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else
            {
                isDiving = true;
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += direction * diveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
            }

            if (Vector3.Distance(transform.position, targetPoint.position) <= collisionThreshold)
            {
                Destroy(gameObject);
                assignedTargets.Remove(targetPoint);
            }
        }
    }

    public void AssignTarget(List<Transform> targets)
    {
        foreach (var target in targets)
        {
            if (!assignedTargets.Contains(target))
            {
                targetPoint = target;
                assignedTargets.Add(target);
                return;
            }
        }
        Destroy(gameObject);
    }
}