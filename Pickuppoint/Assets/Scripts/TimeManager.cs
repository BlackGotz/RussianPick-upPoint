using System;
using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Настройки времени")]
    public float startTime = 9 * 60f;   // 9:00 в минутах
    public float endTime = 21 * 60f;    // 21:00 в минутах
    public float timeIncrement = 1f;    // 1 минута в реальную секунду (можно масштабировать)

    public float currentTime { get; private set; }

    // События, которые могут подписываться другие системы
    public event Action<float> OnTimeUpdated;
    public event Action OnDayEnded;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentTime = startTime;
        StartCoroutine(TimeRoutine());
        Debug.Log("Старт времени");
    }

    IEnumerator TimeRoutine()
    {
        while (currentTime < endTime)
        {
            yield return new WaitForSeconds(1f);
            currentTime += timeIncrement;
            OnTimeUpdated?.Invoke(currentTime);
            Debug.Log(currentTime);
        }
        OnDayEnded?.Invoke();
    }
}
