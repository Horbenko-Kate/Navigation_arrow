using UnityEngine;
using UnityEngine.Android;
using System.Collections;

public class GPSService : MonoBehaviour
{
    public bool IsRunning => Input.location.status == LocationServiceStatus.Running;
    public float CurrentLat => Input.location.lastData.latitude;
    public float CurrentLon => Input.location.lastData.longitude;
    public float Heading => (Input.compass.trueHeading != 0) ? Input.compass.trueHeading : Input.compass.magneticHeading;

    void Start()
    {
        StartCoroutine(SetupLocationAndCompass());
    }

    IEnumerator SetupLocationAndCompass()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            yield return new WaitForSeconds(1);
        }
        Input.location.Start(0.5f, 0.1f);
        Input.compass.enabled = true;
        yield return new WaitForSeconds(1);
    }
}