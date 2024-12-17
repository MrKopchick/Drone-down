using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            // Оновлення тексту для TextMeshPro
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = isSpawning ? "Cancel" : "Spawn";
            }
            else
            {
                Debug.LogError("TMP_Text component not found in button's children.");
            }
        }
    }

    public void ResetButtonState()
    {
        isSpawning = false;
        Debug.Log($"Resetting button state for {gameObject.name}");
        UpdateButtonState();
    }

}