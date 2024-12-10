using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform target;
    public float speed = 20f;
    public float rotationSpeed = 5f;
    public float collisionRadius = 1f;
    public float maxLifetime = 300f;

    private bool isChasing = true;

    void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    void Update()
    {
        if (target == null)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            float angleToTarget = Vector3.Angle(transform.forward, target.position - transform.position);
            if (angleToTarget > 90f)
            {
                isChasing = false;
            }
        }

        transform.position += transform.forward * speed * Time.deltaTime;

        if (isChasing && target != null && Vector3.Distance(transform.position, target.position) <= collisionRadius)
        {
            Destroy(target.gameObject);
            Destroy(gameObject);
        }
    }
}
