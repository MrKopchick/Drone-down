using UnityEngine;

public class Shahed : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float diveSpeed = 20f;
    [SerializeField] private float diveDistance = 15f;
    [SerializeField] private float collisionThreshold = 1f;

    private Transform targetBase;
    private bool isDiving = false;

    public void StartHuntingBase(Transform target)
    {
        targetBase = target;
    }

    void Update()
    {
        if (targetBase == null)
        {
            Transform newTarget = GetRandomBase();
            if (newTarget != null)
            {
                targetBase = newTarget;
            }
            else
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                return;
            }
        }

        Vector3 targetXZ = new Vector3(targetBase.position.x, transform.position.y, targetBase.position.z);
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
            Vector3 direction = (targetBase.position - transform.position).normalized;
            transform.position += direction * diveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, targetBase.position) <= collisionThreshold)
        {
            Destroy(targetBase.gameObject);
            Destroy(gameObject);
        }
    }

    private Transform GetRandomBase()
    {
        BaseSpawner baseSpawner = FindObjectOfType<BaseSpawner>();
        if (baseSpawner == null) return null;

        var bases = baseSpawner.GetExistingBases();
        if (bases.Count == 0) return null;

        return bases[Random.Range(0, bases.Count)];
    }
}
