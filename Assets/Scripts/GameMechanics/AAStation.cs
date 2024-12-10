using UnityEngine;

public class AAStation : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public float detectionRadius = 50f;
    public float missileSpeed = 20f;
    public float missileLifetime = 5f;

    private GameObject targetPlane;
    private bool missileLaunched = false;

    void Update()
    {
        if (targetPlane == null)
        {
            FindClosestPlane();
            missileLaunched = false; // Готовність до атаки нової цілі
        }
        else if (!missileLaunched && Vector3.Distance(transform.position, targetPlane.transform.position) <= detectionRadius)
        {
            LaunchMissile();
        }
        else if (targetPlane != null && Vector3.Distance(transform.position, targetPlane.transform.position) > detectionRadius)
        {
            targetPlane = null;
        }
    }

    void FindClosestPlane()
    {
        GameObject[] planes = GameObject.FindGameObjectsWithTag("Plane");
        float closestDistance = detectionRadius;

        foreach (GameObject plane in planes)
        {
            float distance = Vector3.Distance(transform.position, plane.transform.position);
            if (distance <= closestDistance)
            {
                targetPlane = plane;
                closestDistance = distance;
            }
        }
    }

    void LaunchMissile()
    {
        GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
        Missile missileScript = missile.GetComponent<Missile>();
        missileScript.target = targetPlane.transform;
        missileScript.speed = missileSpeed;
        missileLaunched = true;
        Destroy(missile, missileLifetime);
    }
}