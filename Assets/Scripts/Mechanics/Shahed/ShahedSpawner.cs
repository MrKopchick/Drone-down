using UnityEngine;

public class ShahedSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject shahedPrefab;  // Префаб шахеда
    [SerializeField] private Transform[] spawnPoints;  // Точки для спавну шахедів
    [SerializeField] private float spawnInterval = 5f; // Інтервал спавну шахедів

    [Header("Targets")]
    [SerializeField] private BaseSpawner baseSpawner;  // Для доступу до баз
    private Transform[] baseTargets;  // Список всіх баз

    private void Start()
    {
        // Встановлюємо інтервал для спавну шахедів
        InvokeRepeating("SpawnShahed", 0f, spawnInterval);
    }

    private void Update()
    {
        // Оновлюємо список баз на кожному кадрі
        baseTargets = baseSpawner.GetExistingBases().ToArray();
    }

    private void SpawnShahed()
    {
        // Якщо є бази, вибираємо одну випадкову для атаки
        if (baseTargets.Length > 0)
        {
            // Вибираємо випадкову цільову базу
            Transform targetBase = baseTargets[Random.Range(0, baseTargets.Length)];

            // Вибираємо випадкову точку для спавну
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Спавнимо шахеда
            GameObject newShahed = Instantiate(shahedPrefab, spawnPoint.position, spawnPoint.rotation);
            Shahed shahedScript = newShahed.GetComponent<Shahed>();

            // Починаємо переслідування випадкової бази
            shahedScript.StartHuntingBase(targetBase);  // Передаємо правильний аргумент типу Transform
        }
    }
}