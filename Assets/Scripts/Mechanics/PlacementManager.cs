using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private BaseSpawner baseSpawner;
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        if (baseSpawner == null || mainCamera == null) return;

        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return; // Не робимо Raycast, якщо миша поза екраном
        }

        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, baseSpawner.PlacementLayer))
        {
            var isValid = baseSpawner.CanPlaceBase(hit.point);
            baseSpawner.UpdatePreview(hit.point, isValid);

            if (Input.GetMouseButtonDown(0) && isValid)
            {
                baseSpawner.PlaceBase(hit.point);
                baseSpawner.RemovePreview();
            }
        }
        else
        {
            baseSpawner.RemovePreview();
        }
    }
}