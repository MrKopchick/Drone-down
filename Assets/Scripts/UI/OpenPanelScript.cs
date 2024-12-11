using UnityEngine;
using DG.Tweening;

public class OpenPanelScript : MonoBehaviour
{
    [Header("Panel Settings")]
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private Transform finalPosition;
    [SerializeField] private Transform startPosition;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private bool horizontal = true;
    [SerializeField] private bool vertical = false;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform spawnPosition;

    private bool isActivated = false;

    public void TogglePanel()
    {
        if (isActivated)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
            SpawnObject();
        }
    }

    private void OpenPanel()
    {
        MovePanel(finalPosition);
        isActivated = true;
    }

    private void ClosePanel()
    {
        MovePanel(startPosition);
        isActivated = false;
    }

    private void MovePanel(Transform targetPosition)
    {
        if (panelRectTransform == null)
        {
            Debug.LogError("panelRectTransform is not assigned.");
            return;
        }

        if (panelRectTransform.parent == null)
        {
            Debug.LogError("panelRectTransform has no parent.");
            return;
        }

        if (targetPosition == null)
        {
            Debug.LogError("targetPosition is not assigned.");
            return;
        }

        Vector3 localPosition = panelRectTransform.parent.InverseTransformPoint(targetPosition.position);

        if (horizontal)
        {
            panelRectTransform.DOAnchorPosX(localPosition.x, duration);
        }
        if (vertical)
        {
            panelRectTransform.DOAnchorPosY(localPosition.y, duration);
        }
    }


    private void SpawnObject()
    {
        if (objectToSpawn != null && spawnPosition != null)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition.position, spawnPosition.rotation, this.transform);
            spawnedObject.transform.localPosition = spawnPosition.localPosition;
        }
    }
}