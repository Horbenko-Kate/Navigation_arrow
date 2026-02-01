using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class GPSDirectionUI : MonoBehaviour
{
    [Header("Сервіси")]
    public GPSService gpsService;

    [Header("UI Панелі")]
    public GameObject menuPanel;
    public GameObject menuButton;
    public GameObject arrivalPanel;
    public InputField latInput;
    public InputField lonInput;
    public Text debugText;

    [Header("Налаштування стрілки")]
    public Transform arrowTransform;
    public Transform arrowLineTransform;
    public float directionOffset = 0f;
    public float arrivalThreshold = 5f;

    private float destLat, destLon;
    private bool hasArrived = false;

    void Start()
    {
        latInput.text = "48.51619";
        lonInput.text = "32.25826";

        if (menuPanel != null) menuPanel.SetActive(false);
        if (arrivalPanel != null) arrivalPanel.SetActive(false);
        if (menuButton != null) menuButton.SetActive(true);

        UpdateCoordinates();
    }

    void Update()
    {
        if (gpsService == null || !gpsService.IsRunning) return;

        float curLat = gpsService.CurrentLat;
        float curLon = gpsService.CurrentLon;
        float phoneHeading = gpsService.Heading;

        float targetBearing = NavigationMath.CalculateBearing(curLat, curLon, destLat, destLon);
        float distance = NavigationMath.CalculateDistance(curLat, curLon, destLat, destLon);

        if (distance <= arrivalThreshold && !hasArrived)
        {
            hasArrived = true;
            if (arrivalPanel != null) arrivalPanel.SetActive(true);
        }

        float finalRotation = (targetBearing - phoneHeading) + directionOffset;
        float smoothY = Mathf.LerpAngle(arrowTransform.localEulerAngles.y, finalRotation, Time.deltaTime * 5f);

        Quaternion finalRot = Quaternion.Euler(0, smoothY, 0);
        arrowTransform.localRotation = finalRot;
        if (arrowLineTransform != null) arrowLineTransform.localRotation = finalRot;

        string distText = distance > 1000 ? $"{(distance / 1000f):F2} км" : $"{(int)distance} м";
        string phoneDirection = NavigationMath.GetCardinalDirection(phoneHeading);

        debugText.text =
             $"Напрямок телефону: {phoneDirection} ({phoneHeading:F0}°)\n" +
             $"Кут до цілі: {targetBearing:F0}°\n" +
             $"Відстань до цілі: {distText}\n" +
             $"Мої координати: {curLat:F5} {curLon:F5}";
    }

    public void UpdateCoordinates()
    {
        if (float.TryParse(latInput.text, NumberStyles.Any, CultureInfo.InvariantCulture, out destLat) &&
            float.TryParse(lonInput.text, NumberStyles.Any, CultureInfo.InvariantCulture, out destLon))
        {
            if (menuPanel.activeSelf) ToggleMenu();
            hasArrived = false;
        }
    }

    public void ToggleMenu()
    {
        bool state = !menuPanel.activeSelf;
        menuPanel.SetActive(state);
        if (menuButton != null) menuButton.SetActive(!state);
    }

    public void CloseArrivalPanel() => arrivalPanel?.SetActive(false);
}