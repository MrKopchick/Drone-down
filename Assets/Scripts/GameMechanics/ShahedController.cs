using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahedController : MonoBehaviour
{
    public Transform[] targetPoints;
    public float speed = 10f;
    public float diveSpeed = 20f;
    public float diveDistance = 15f;
    public float collisionThreshold = 1f;

    private Transform targetPoint;
    private bool isDiving = false;

    void Start()
    {
        if (targetPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, targetPoints.Length);
            targetPoint = targetPoints[randomIndex];
        }
    }

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
            }
        }
    }
}
