using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Ёлементы UI")]
    public TextMeshProUGUI timeText;
    public GameObject phoneDisplay;
    public TextMeshPro phoneText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        TimeManager.Instance.OnTimeUpdated += UpdateTimeUI;
        HidePhoneDisplay();
    }

    private void UpdateTimeUI(float currentTime)
    {
        int hours = Mathf.FloorToInt(currentTime / 60);
        int minutes = Mathf.FloorToInt(currentTime % 60);
        timeText.text = string.Format("{0:00}:{1:00}", hours, minutes);
    }

    public void ShowClientPhone(int orderNumber)
    {
        if (phoneDisplay != null)
            phoneDisplay.SetActive(true);
        if (phoneText != null)
            phoneText.text = orderNumber.ToString();
    }

    public void HidePhoneDisplay()
    {
        if (phoneDisplay != null)
            phoneDisplay.SetActive(false);
    }
}
