using UnityEngine;

public class Shahed : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float diveSpeed = 20f;
    [SerializeField] private float diveDistance = 15f;
    [SerializeField] private float collisionThreshold = 1f;
    [SerializeField] private float wobbleIntensity = 0.5f;
    [SerializeField] private float wobbleFrequency = 2f;
    [SerializeField] private float randomDirectionChangeInterval = 2f;

    private Transform targetBase;
    private bool isDiving = false;
    private float timeSinceDirectionChange = 0f;
    private Vector3 currentDirection;

    public void StartHuntingBase(Transform target)
    {
        targetBase = target;
    }

    void Start()
    {
        ChooseNewRandomDirection();
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
                Wander();
                return;
            }
        }

        Vector3 targetXZ = new Vector3(targetBase.position.x, transform.position.y, targetBase.position.z);
        float distanceXZ = Vector3.Distance(transform.position, targetXZ);

        if (!isDiving && distanceXZ > diveDistance)
        {
            Vector3 direction = (targetXZ - transform.position).normalized;
            AddWobble(ref direction);
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

    private void Wander()
    {
        timeSinceDirectionChange += Time.deltaTime;

        if (timeSinceDirectionChange >= randomDirectionChangeInterval)
        {
            ChooseNewRandomDirection();
            timeSinceDirectionChange = 0f;
        }

        Vector3 direction = currentDirection;
        AddWobble(ref direction);
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void ChooseNewRandomDirection()
    {
        float randomAngle = Random.Range(0f, 360f);
        currentDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle)).normalized;
    }

    private void AddWobble(ref Vector3 direction)
    {
        float wobbleOffset = Mathf.Sin(Time.time * wobbleFrequency) * wobbleIntensity;
        Vector3 wobble = new Vector3(wobbleOffset, 0, wobbleOffset);
        direction += wobble;
        direction.Normalize();
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