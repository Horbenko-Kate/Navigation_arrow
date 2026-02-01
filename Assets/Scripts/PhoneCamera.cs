using UnityEngine;
using UnityEngine.UI;

public partial class PhoneCamera : MonoBehaviour
{
    private WebCamTexture backCam;
    public RawImage background; // Сюди перетягнемо наш RawImage
    public AspectRatioFitter fitter; // Щоб картинка не розтягувалася криво

    void Start()
    {
        // Шукаємо задню камеру пристрою
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("Камеру не знайдено");
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("Не вдалося знайти задню камеру");
            return;
        }

        backCam.Play(); // Вмикаємо камеру
        background.texture = backCam; // Передаємо картинку на RawImage
    }

    void Update()
    {
        if (backCam == null) return;

        // Коригуємо орієнтацію (камера на телефонах часто повернута на 90 градусів)
        float ratio = (float)backCam.width / (float)backCam.height;
        fitter.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
}