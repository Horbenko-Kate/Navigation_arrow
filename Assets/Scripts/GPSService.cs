using UnityEngine;
using UnityEngine.Android;
using System.Collections;

public class GPSService : MonoBehaviour
{
    // Public properties to access GPS and Compass data from other scripts
    public bool IsRunning => Input.location.status == LocationServiceStatus.Running;
    public float CurrentLat => Input.location.lastData.latitude;
    public float CurrentLon => Input.location.lastData.longitude;

    // Returns true heading if available, otherwise falls back to magnetic heading
    public float Heading => (Input.compass.trueHeading != 0) ? Input.compass.trueHeading : Input.compass.magneticHeading;

    // Initializes the GPS setup process on startup
    void Start()
    {
        StartCoroutine(SetupLocationAndCompass());
    }

    // Coroutine to request permissions and initialize hardware sensors
    IEnumerator SetupLocationAndCompass()
    {
        // Check and request location permissions for Android devices
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(1); // Wait for the user to respond to the prompt
        }

        // Start the location service with 0.5m accuracy and 0.1m update distance
        Input.location.Start(0.5f, 0.1f);

        // Enable the digital compass (magnetometer)
        Input.compass.enabled = true;

        // Brief delay to allow sensors to initialize
        yield return new WaitForSeconds(1);
    }
}