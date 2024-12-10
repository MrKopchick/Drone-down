using UnityEngine;

public class Shahed : MonoBehaviour
{
    public Transform targetBase;  // Ціль для шахеда
    public float speed = 10f;
    public float diveSpeed = 20f;
    public float diveDistance = 15f;
    public float collisionThreshold = 1f;

    private bool isDiving = false;

    void Update()
    {
        if (targetBase != null)
        {
            Vector3 targetXZ = new Vector3(targetBase.position.x, transform.position.y, targetBase.position.z);
            float distanceXZ = Vector3.Distance(transform.position, targetXZ);

            // Летимо прямо до бази
            if (!isDiving && distanceXZ > diveDistance)
            {
                Vector3 direction = (targetXZ - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            // Починаємо пікірування
            else
            {
                isDiving = true;
                Vector3 direction = (targetBase.position - transform.position).normalized;
                transform.position += direction * diveSpeed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(direction);
            }

            // Перевірка на зіткнення з базою
            if (Vector3.Distance(transform.position, targetBase.position) <= collisionThreshold)
            {
                Destroy(targetBase.gameObject);  // Знищуємо базу
                Destroy(gameObject);  // Знищуємо шахеда
            }
        }
    }

    public void StartHuntingBase(Transform target)
    {
        targetBase = target;  
    }
}