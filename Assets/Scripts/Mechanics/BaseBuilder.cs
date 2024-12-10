using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private float buildTime = 15f;
    [SerializeField] private Material inProgressMaterial;
    [SerializeField] private Material completedMaterial;

    private float buildProgress;
    private bool isComplete;
    private Renderer baseRenderer;

    public bool IsComplete => isComplete;

    public void Initialize()
    {
        baseRenderer = GetComponent<Renderer>();
        if (baseRenderer != null && inProgressMaterial != null) baseRenderer.material = inProgressMaterial;
        isComplete = false;
    }

    private void Update()
    {
        if (isComplete) return;
        buildProgress += Time.deltaTime;
        if (buildProgress >= buildTime) CompleteConstruction();
    }

    private void CompleteConstruction()
    {
        isComplete = true;
        if (baseRenderer != null && completedMaterial != null) baseRenderer.material = completedMaterial;
    }
}