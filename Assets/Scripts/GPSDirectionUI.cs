using UnityEngine;

public class GPSDirectionManager : MonoBehaviour
{
    [Header("UI ≈лементи")]
    public GameObject menuPanel;     // Ўторка меню
    public GameObject menuButton;    //  нопка в≥дкритт€ меню
    public GameObject backButton;    //  нопка назад

    public void ToggleMenu()
    {
        bool isOpening = !menuPanel.activeSelf; // «м≥нюЇмо стан на протилежний
        menuPanel.SetActive(isOpening);
        if (menuButton != null) menuButton.SetActive(!isOpening); // ’оваЇмо кнопку, €кщо меню в≥дкрите
    }
}

