using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject previewBasePrefab;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float minDistanceBetweenBases = 10f;

    private GameObject previewBase;
    private List<Transform> existingBases = new List<Transform>();

    public List<Transform> BaseTargets { get; private set; } = new List<Transform>();

    public LayerMask PlacementLayer => placementLayer;

    public void UpdatePreview(Vector3 position, bool isValid)
    {
        if (previewBase == null) previewBase = Instantiate(previewBasePrefab);

        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity, placementLayer))
        {
            previewBase.transform.position = hit.point;  // Встановлення на поверхню
            previewBase.transform.rotation = Quaternion.FromToRotation(previewBase.transform.up, hit.normal) * previewBase.transform.rotation;  // Оновлення орієнтації
            Renderer renderer = previewBase.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            }
        }
        else
        {
            previewBase.transform.position = position;  // Default position if no hit
        }
    }

    public void RemovePreview()
    {
        if (previewBase != null) Destroy(previewBase);
    }

    public bool CanPlaceBase(Vector3 position)
    {
        foreach (var existingBase in existingBases)
        {
            if (Vector3.Distance(position, existingBase.position) < minDistanceBetweenBases) return false;
        }
        return true;
    }

    public void PlaceBase(Vector3 position, Quaternion rotation)
    {
        var newBase = Instantiate(basePrefab, position, rotation);
        existingBases.Add(newBase.transform);
        BaseTargets.Add(newBase.transform);

        var baseBuilder = newBase.GetComponent<BaseBuilder>();
        baseBuilder.Initialize();
    }

}
