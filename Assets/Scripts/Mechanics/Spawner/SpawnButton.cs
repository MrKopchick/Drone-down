using UnityEngine;
using UnityEngine.UI;

public class SpawnButton : MonoBehaviour
{
    [SerializeField] private SpawnableObject spawnableObject;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Color spawnColor = Color.green;
    [SerializeField] private Color cancelColor = Color.red;

    private Button button;
    private bool isSpawning = false;
    private Image buttonImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        UpdateButtonState();
    }

    private void OnButtonClick()
    {
        if (isSpawning)
        {
            spawnManager.SetSpawnableObject(null);
        }
        else
        {
            spawnManager.SetSpawnableObject(spawnableObject);
        }

        isSpawning = !isSpawning;
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (buttonImage != null)
        {
            buttonImage.color = isSpawning ? cancelColor : spawnColor;
        }

        if (button != null)
        {
            button.GetComponentInChildren<Text>().text = isSpawning ? "Cancel" : "Spawn";
        }
    }

    public void ResetButtonState()
    {
        isSpawning = false;
        UpdateButtonState();
    }
}