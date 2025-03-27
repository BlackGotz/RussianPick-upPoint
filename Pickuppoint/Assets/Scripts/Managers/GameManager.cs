using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // ������������� �� ��������� ���
        TimeManager.Instance.OnDayEnded += EndDay;
    }

    private void EndDay()
    {
        Debug.Log("���� ��������");
        // ����� ����� �������� �������������� ������ ���������� ���
    }
}
