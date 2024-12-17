using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    public static BaseSpawner Instance { get; private set; }

    private List<Transform> existingBases = new List<Transform>();

    private void Awake()
    {
        // Переконаємось, що цей об'єкт є єдиним інстансом
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterBase(Transform baseTransform)
    {
        if (!existingBases.Contains(baseTransform))
        {
            existingBases.Add(baseTransform);
        }
    }

    public void UnregisterBase(Transform baseTransform)
    {
        if (existingBases.Contains(baseTransform))
        {
            existingBases.Remove(baseTransform);
        }
    }

    public List<Transform> GetExistingBases()
    {
        return existingBases;
    }

    // Метод для спавну бази (якщо потрібно)
    public void SpawnBase(GameObject basePrefab, Vector3 position, Quaternion rotation)
    {
        GameObject newBase = Instantiate(basePrefab, position, rotation);
        RegisterBase(newBase.transform); // Додаємо базу до списку
    }
}