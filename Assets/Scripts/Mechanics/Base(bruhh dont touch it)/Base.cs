using UnityEngine;

public class Base : MonoBehaviour
{
    private void Start()
    {
        // Додаємо базу до BaseSpawner одразу після спавну
        BaseSpawner.Instance.RegisterBase(this.transform);
    }

    private void OnDestroy()
    {
        // Видаляємо базу зі списку, якщо вона знищена
        BaseSpawner.Instance.UnregisterBase(this.transform);
    }
}