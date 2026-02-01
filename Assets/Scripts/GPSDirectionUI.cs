using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class UIManager : MonoBehaviour
{
    [Header("UI Елементи")]
    public GameObject MenuPanel;
    public GameObject menuButton;
    public InputField latInput;
    public InputField lonInput;
    public GameObject arrivalPanel;

    private float destinationLat;
    private float destinationLon;

    void Start()
    {
        if (MenuPanel != null) MenuPanel.SetActive(false);
        if (menuButton != null) menuButton.SetActive(true);

    }

    public void ToggleMenu()
    {
        if (MenuPanel == null) return;

        bool isOpening = !MenuPanel.activeSelf;
        MenuPanel.SetActive(isOpening);
        if (menuButton != null)
        {
            menuButton.SetActive(!isOpening);
        }
    }

    public void UpdateCoordinates()
    {
        if (float.TryParse(latInput.text, NumberStyles.Any, CultureInfo.InvariantCulture, out destinationLat) &&
            float.TryParse(lonInput.text, NumberStyles.Any, CultureInfo.InvariantCulture, out destinationLon))
        {
            Debug.Log("Координати прийнято. Широта: " + destinationLat + ", Довгота: " + destinationLon);

            if (MenuPanel.activeSelf)
            {
                ToggleMenu();
            }
        }
        else
        {
            Debug.LogError("Помилка формату координат!");
        }
    }
}