using System;
using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("��������� �������")]
    public float startTime = 9 * 60f;   // 9:00 � �������
    public float endTime = 21 * 60f;    // 21:00 � �������
    public float timeIncrement = 1f;    // 1 ������ � �������� ������� (����� ��������������)

    public float currentTime { get; private set; }

    // �������, ������� ����� ������������� ������ �������
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
        Debug.Log("����� �������");
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
