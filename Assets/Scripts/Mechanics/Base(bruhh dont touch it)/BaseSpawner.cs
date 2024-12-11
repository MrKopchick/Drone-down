using UnityEngine;
using System.Collections.Generic;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject previewBasePrefab;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float minDistanceBetweenBases = 10f;
    [SerializeField] private float maxSlopeAngle = 30f;

    private GameObject previewBase;
    private List<Transform> existingBases = new List<Transform>();

    public LayerMask PlacementLayer => placementLayer;

    private const string BaseKeyPrefix = "Base_";

    public List<Transform> GetExistingBases()
    {
        return existingBases;
    }
    
    void Update()
    {
        existingBases.RemoveAll(baseTransform => baseTransform == null);
    }

    public void UpdatePreview(Vector3 position, bool isValid)
    {
        if (previewBase == null)
            previewBase = Instantiate(previewBasePrefab);

        previewBase.transform.position = position;
        
        Renderer renderer = previewBase.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }
    }

    public void RemovePreview()
    {
        if (previewBase != null)
        {
            Destroy(previewBase);
        }
    }

    public bool CanPlaceBase(Vector3 position)
    {
        foreach (var existingBase in existingBases)
        {
            if (existingBase != null && Vector3.Distance(position, existingBase.position) < minDistanceBetweenBases)
            {
                return false;
            }
        }

        return !IsBaseTooSteep(position);
    }

    private bool IsBaseTooSteep(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out hit, 20f, placementLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            return slopeAngle > maxSlopeAngle;
        }

        return false;
    }

    public void PlaceBase(Vector3 position, Quaternion rotation)
    {
        var newBase = Instantiate(basePrefab, position, rotation);
        existingBases.Add(newBase.transform);

        SaveBasePosition(newBase.transform);
        NotifyShaheds();
    }

    private void SaveBasePosition(Transform baseTransform)
    {
        string baseKey = BaseKeyPrefix + existingBases.Count;
        Vector3 position = baseTransform.position;
        Quaternion rotation = baseTransform.rotation;

        PlayerPrefs.SetFloat(baseKey + "_X", position.x);
        PlayerPrefs.SetFloat(baseKey + "_Y", position.y);
        PlayerPrefs.SetFloat(baseKey + "_Z", position.z);
        PlayerPrefs.SetFloat(baseKey + "_RotY", rotation.eulerAngles.y);
        PlayerPrefs.Save();
    }

    public void LoadBases()
    {
        existingBases.Clear();
        int index = 0;
        while (PlayerPrefs.HasKey(BaseKeyPrefix + index))
        {
            float x = PlayerPrefs.GetFloat(BaseKeyPrefix + index + "_X");
            float y = PlayerPrefs.GetFloat(BaseKeyPrefix + index + "_Y");
            float z = PlayerPrefs.GetFloat(BaseKeyPrefix + index + "_Z");
            float rotY = PlayerPrefs.GetFloat(BaseKeyPrefix + index + "_RotY");

            Vector3 position = new Vector3(x, y, z);
            Quaternion rotation = Quaternion.Euler(0, rotY, 0);

            var newBase = Instantiate(basePrefab, position, rotation);
            existingBases.Add(newBase.transform);

            index++;
        }
    }
    
    public void NotifyShaheds()
    {
        Shahed[] shaheds = FindObjectsOfType<Shahed>();
        
        foreach (Shahed shahed in shaheds)
        {
            if (existingBases.Count > 0)
            {
                shahed.StartHuntingBase(existingBases[0]); 
            }
        }
    }
}
