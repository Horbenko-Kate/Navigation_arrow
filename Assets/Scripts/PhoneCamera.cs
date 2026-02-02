using UnityEngine;
using UnityEngine.UI;

public partial class PhoneCamera : MonoBehaviour
{
    // Variables for the camera feed and UI display
    private WebCamTexture backCam;
    public RawImage background;      // UI element to show the camera feed
    public AspectRatioFitter fitter; // Keeps the image proportions correct

    void Start()
    {
        // 1. Get a list of all available camera devices on the phone
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera found");
            return;
        }

        // 2. Loop through devices to find the back-facing camera
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                // Initialize the camera texture with screen resolution
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        // 3. Start the camera and link it to the UI RawImage
        backCam.Play();
        background.texture = backCam;
    }

    void Update()
    {
        if (backCam == null) return;

        // 4. Adjust the AspectRatioFitter to prevent stretching
        float ratio = (float)backCam.width / (float)backCam.height;
        fitter.aspectRatio = ratio;

        // 5. Check if the video is mirrored (common on some devices) and flip if needed
        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        // 6. Fix the image rotation (phone cameras are often rotated by 90 degrees)
        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}