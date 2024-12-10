using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    [SerializeField] private BaseSpawner baseSpawner;
    [SerializeField] private Camera mainCamera;

    private void Update()
    {
        if (baseSpawner == null || mainCamera == null) return;

        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < 0 || mousePos.y < 0 || mousePos.x > Screen.width || mousePos.y > Screen.height)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, baseSpawner.PlacementLayer))
        {
            bool isValid = baseSpawner.CanPlaceBase(hit.point);
            baseSpawner.UpdatePreview(hit.point, isValid);

            if (Input.GetMouseButtonDown(0) && isValid)
            {
                Quaternion rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
                baseSpawner.PlaceBase(hit.point, rotation);
                baseSpawner.RemovePreview();
            }
        }
        else
        {
            baseSpawner.RemovePreview();
        }
    }
}