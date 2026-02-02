using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class GPSDirectionUI : MonoBehaviour
{
    [Header("Services")]
    // Reference to the sensor service script
    public GPSService gpsService;

    [Header("UI Panels")]
    // References to different UI elements in the Canvas
    public GameObject menuPanel;      // The sliding settings menu
    public GameObject menuButton;     // The button that opens the menu
    public GameObject arrivalPanel;   // The "You have arrived" pop-up window
    public InputField latInput;       // Input field for Latitude
    public InputField lonInput;       // Input field for Longitude
    public Text debugText;            // Text field to show GPS data on screen

    [Header("Arrow Settings")]
    // References for the 3D visual arrow
    public Transform arrowTransform;      // The main 3D arrow object
    public Transform arrowLineTransform;  // The arrow's outline/glow effect
    public float directionOffset = 0f;    // Manual adjustment for arrow rotation
    public float arrivalThreshold = 5f;   // Distance in meters to trigger arrival

    private float destLat, destLon;       // Storage for target coordinates
    private bool hasArrived = false;      // Flag to prevent the arrival window from flickering

    // Runs once when the application starts
    void Start()
    {
        // Set default coordinates (Kovalivskyi Park)
        latInput.text = "48.51619";
        lonInput.text = "32.25826";

        // Set initial visibility of UI elements
        if (menuPanel != null) menuPanel.SetActive(false);
        if (arrivalPanel != null) arrivalPanel.SetActive(false);
        if (menuButton != null) menuButton.SetActive(true);

        // Load the initial coordinates from the input fields
        UpdateCoordinates();
    }

    // Runs every frame (main logic loop)
    void Update()
    {
        // Stop if the GPS service is missing or not working yet
        if (gpsService == null || !gpsService.IsRunning) return;

        // Get current data from the GPS sensor script
        float curLat = gpsService.CurrentLat;
        float curLon = gpsService.CurrentLon;
        float phoneHeading = gpsService.Heading;

        // Use math functions to find the angle and distance to the target
        float targetBearing = NavigationMath.CalculateBearing(curLat, curLon, destLat, destLon);
        float distance = NavigationMath.CalculateDistance(curLat, curLon, destLat, destLon);

        // Check if the user reached the destination
        if (distance <= arrivalThreshold && !hasArrived)
        {
            hasArrived = true;
            if (arrivalPanel != null) arrivalPanel.SetActive(true);
        }

        // Calculate arrow rotation (target direction minus phone direction)
        float finalRotation = (targetBearing - phoneHeading) + directionOffset;

        // Smoothly rotate the arrow to avoid jerky movements
        float smoothY = Mathf.LerpAngle(arrowTransform.localEulerAngles.y, finalRotation, Time.deltaTime * 5f);

        // Apply rotation to both the arrow and its outline
        Quaternion finalRot = Quaternion.Euler(0, smoothY, 0);
        arrowTransform.localRotation = finalRot;
        if (arrowLineTransform != null) arrowLineTransform.localRotation = finalRot;

        // Format distance text to show meters or kilometers
        string distText = distance > 1000 ? $"{(distance / 1000f):F2} km" : $"{(int)distance} m";

        // Convert degrees to cardinal directions (e.g., North, East)
        string phoneDirection = NavigationMath.GetCardinalDirection(phoneHeading);

        // Update the screen with the latest navigation data
        debugText.text =
                     $"Phone Direction: {phoneDirection}\n" +
                     $"Distance to Target: {distText}\n" +
                     $"My Coordinates:\n          Lat: {curLat:F5}\n          Lon: {curLon:F5}";
    }

    // Read coordinates from Input Fields and save them
    public void UpdateCoordinates()
    {
        // Use InvariantCulture to handle dots/commas correctly on different phones
        if (float.TryParse(latInput.text, NumberStyles.Any, CultureInfo.InvariantCulture, out destLat) &&
            float.TryParse(lonInput.text, NumberStyles.Any, CultureInfo.InvariantCulture, out destLon))
        {
            // Close menu after updating and reset arrival state for new target
            if (menuPanel.activeSelf) ToggleMenu();
            hasArrived = false;
        }
    }

    // Switch the menu panel ON and OFF
    public void ToggleMenu()
    {
        bool state = !menuPanel.activeSelf;
        menuPanel.SetActive(state);
        // Hide the open button when the menu is visible
        if (menuButton != null) menuButton.SetActive(!state);
    }

    // Hide the arrival window when the user clicks a button
    public void CloseArrivalPanel() => arrivalPanel?.SetActive(false);
}