using UnityEngine;
using UnityEngine.EventSystems;
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private float minDistanceBetweenObjects = 10f;
    [SerializeField] private float maxSlopeAngle = 30f;
    
    private SpawnButton activeButton;
    private SpawnableObject currentSpawnableObject;
    private GameObject previewObject;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }
    
    public void SetSpawnableObject(SpawnableObject spawnableObject, SpawnButton button = null)
    {
        if (activeButton != null && activeButton != button)
        {
            activeButton.ResetButtonState(); 
        }

        currentSpawnableObject = spawnableObject;
        activeButton = button;

        // Оновлюємо прев'ю
        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        if (currentSpawnableObject != null && currentSpawnableObject.previewPrefab != null)
        {
            previewObject = Instantiate(currentSpawnableObject.previewPrefab);
        }
    }



    private void Update()
    {
        if (currentSpawnableObject == null || mainCamera == null) return;

        // Перевірка, чи курсор над UI
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x > Screen.width || mousePos.y > Screen.height)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            bool isValid = CanPlaceObject(hit.point);
            UpdatePreview(hit.point, isValid);

            if (Input.GetMouseButtonDown(0) && isValid)
            {
                PlaceObject(hit.point);
            }
        }
        else
        {
            RemovePreview();
        }
    }

    private bool CanPlaceObject(Vector3 position)
    {
        if (Physics.Raycast(position + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, placementLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > maxSlopeAngle) return false;
        }

        return true;
    }

    private void UpdatePreview(Vector3 position, bool isValid)
    {
        if (previewObject == null) return;

        previewObject.transform.position = position;

        Renderer renderer = previewObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }
    }

    private void RemovePreview()
    {
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    private void PlaceObject(Vector3 position)
    {
        if (currentSpawnableObject == null || currentSpawnableObject.prefab == null) return;
        
        Instantiate(currentSpawnableObject.prefab, position, Quaternion.identity);
        RemovePreview();
        if (activeButton != null)
        {
            activeButton.ResetButtonState(); 
        }
        SetSpawnableObject(null);
    }



}
