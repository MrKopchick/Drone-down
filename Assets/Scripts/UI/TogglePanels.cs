using UnityEngine;
using UnityEngine.UI;

public class TogglePanels : MonoBehaviour
{
    public GameObject panel1; // Панель 1
    public GameObject panel2; // Панель 2

    public Button button1; // Кнопка 1
    public Button button2; // Кнопка 2

    public Color activeColor = Color.green; // Колір активної кнопки
    public Color inactiveColor = Color.white; // Колір неактивної кнопки

    private void Start()
    {
        // Ініціалізація стану панелей
        SetPanelState(panel1, button1, true);
        SetPanelState(panel2, button2, false);

        // Додавання обробників кліків
        button1.onClick.AddListener(() => SwitchPanel(panel1, button1, panel2, button2));
        button2.onClick.AddListener(() => SwitchPanel(panel2, button2, panel1, button1));
    }

    private void SwitchPanel(GameObject activatePanel, Button activateButton, GameObject deactivatePanel, Button deactivateButton)
    {
        // Встановлюємо стан для першої панелі та кнопки
        SetPanelState(activatePanel, activateButton, true);
        // Встановлюємо стан для другої панелі та кнопки
        SetPanelState(deactivatePanel, deactivateButton, false);
    }

    private void SetPanelState(GameObject panel, Button button, bool isActive)
    {
        // Вмикаємо/вимикаємо панель
        panel.SetActive(isActive);

        // Отримуємо поточні кольори кнопки
        var colors = button.colors;

        // Оновлюємо кольори кнопки
        colors.normalColor = isActive ? activeColor : inactiveColor;
        colors.highlightedColor = colors.normalColor; // Колір при наведенні
        colors.pressedColor = Color.Lerp(colors.normalColor, Color.black, 0.3f); // Колір при натисканні
        colors.selectedColor = colors.normalColor; // Колір при виборі

        // Застосовуємо змінені кольори до кнопки
        button.colors = colors;
    }



}