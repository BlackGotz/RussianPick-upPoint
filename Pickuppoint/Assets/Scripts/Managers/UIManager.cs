using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Элементы UI")]
    public List<TextMeshProUGUI> textList;
    public TextMeshProUGUI finishText;
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
        TimeManager.Instance.OnDayEnded += FinishDay;
    }

    private void UpdateTimeUI(float currentTime)
    {
        int hours = Mathf.FloorToInt(currentTime / 60);
        int minutes = Mathf.FloorToInt(currentTime % 60);
        foreach (var item in textList)
        {
            item.text = string.Format("{0:00}:{1:00}", hours, minutes);
        }
    }

    public void ShowClientPhone(string orderNumber)
    {
        if (phoneDisplay != null)
            phoneDisplay.SetActive(true);
        if (phoneText != null)
            phoneText.text = orderNumber;
    }

    public void HidePhoneDisplay()
    {
        if (phoneDisplay != null)
            phoneDisplay.SetActive(false);
    }

    public void FinishDay()
    {
        finishText.text = "Завершите смену)";
    }
}
