using System.Collections.Generic;
using UnityEngine;

public class ShahedSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shahedPrefab;
    [SerializeField] private float spawnRate = 5f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnRadius = 10f;
    [SerializeField] private BaseManager baseManager;

    private float spawnCooldown;

    void Update()
    {
        spawnCooldown -= Time.deltaTime;

        if (spawnCooldown <= 0f)
        {
            SpawnShahed();
            spawnCooldown = spawnRate;
        }
    }

    private void SpawnShahed()
    {
        Vector3 spawnPosition = spawnPoint.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = spawnPoint.position.y; // забезпечуємо правильну висоту для спавну

        GameObject shahed = Instantiate(shahedPrefab, spawnPosition, Quaternion.identity);
        List<Transform> availableTargets = new List<Transform>(baseManager.GetCompletedBases().ConvertAll(baseBuilder => baseBuilder.transform));
        
        if (availableTargets.Count > 0)
        {
            ShahedController shahedController = shahed.GetComponent<ShahedController>();
            if (shahedController != null)
            {
                shahedController.AssignTarget(availableTargets);
            }
        }
    }
}