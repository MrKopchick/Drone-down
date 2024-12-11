using UnityEngine;

public class ShahedSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject shahedPrefab;
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private float spawnInterval = 5f;

    [Header("Targets")]
    [SerializeField] private BaseSpawner baseSpawner;
    private Transform[] baseTargets;

    private void Start()
    {
        InvokeRepeating("SpawnShahed", 0f, spawnInterval);
    }

    private void Update()
    {
        baseTargets = baseSpawner.GetExistingBases().ToArray();
    }

    private void SpawnShahed()
    {
        if (baseTargets.Length > 0)
        {
            Transform targetBase = baseTargets[Random.Range(0, baseTargets.Length)];
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject newShahed = Instantiate(shahedPrefab, spawnPoint.position, spawnPoint.rotation);
            Shahed shahedScript = newShahed.GetComponent<Shahed>();
            shahedScript.StartHuntingBase(targetBase);
        }
    }
}