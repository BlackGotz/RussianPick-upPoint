using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("����� �������� � �������")]
    public GameObject clientPrefab;
    public Transform clientSpawnPoint;
    public Transform pickupPoint;
    public GameObject boxPrefab;
    public Transform boxSpawnPoint;

    [Header("��������� �������")]
    public int minBoxes = 3;
    public int maxBoxes = 7;
    public float boxesSpawnDeadline = 17 * 60f; // �� 17:00

    private List<int> availableBoxNumbers = new List<int>();
    private bool isClientActive = false;
    private float timeToNextClient;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // ������������� ����� �������� ��� ������ �������
        timeToNextClient = Random.Range(5f, 10f);

        SpawnBoxes();

        // ������������� �� ���������� �������
        TimeManager.Instance.OnTimeUpdated += OnTimeUpdated;
    }

    private void OnTimeUpdated(float currentTime)
    {
        if (!isClientActive)
        {
            // ��������� ������ �� ��������� ���������� �������
            timeToNextClient -= TimeManager.Instance.timeIncrement; // ��� Time.deltaTime, ���� ��������� �� ��������
            if (timeToNextClient <= 0f && availableBoxNumbers.Count > 0)
            {
                SpawnClient();
            }
        }
    }

    private void SpawnBoxes()
    {
        int boxesCount = Random.Range(minBoxes, maxBoxes + 1);
        Debug.Log("���������� �������" + boxesCount);
        for (int i = 0; i < boxesCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * 1f;
            Vector3 spawnPosition = boxSpawnPoint.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            GameObject boxObj = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
            int boxNumber = Random.Range(10000, 99999);
            Box box = boxObj.GetComponent<Box>();
            box.SetupBox(boxNumber);
            availableBoxNumbers.Add(boxNumber);
        }
    }

    private void SpawnClient()
    {
        // �������� ��������� ����� ������� ��� �������
        int index = Random.Range(0, availableBoxNumbers.Count);
        int requiredNumber = availableBoxNumbers[index];

        // ��������� ����������� ������ �������, ����� �� ������� � ������� ����� ������
        Vector3 direction = (pickupPoint.position - clientSpawnPoint.position).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(direction);

        GameObject clientObj = Instantiate(clientPrefab, clientSpawnPoint.position, spawnRotation);
        Client client = clientObj.GetComponent<Client>();
        client.SetupClient(requiredNumber, pickupPoint);

        isClientActive = true;
    }

    /// <summary>
    /// ���������� ����� ������ ������
    /// </summary>
    public void ClientFinished(int deliveredBoxNumber)
    {
        isClientActive = false;
        // ������� ����� ������� �� ���������
        availableBoxNumbers.Remove(deliveredBoxNumber);
        // ���������� ����� ����� �������� �� ��������� ���������� �������
        timeToNextClient = Random.Range(5f, 10f);
        UIManager.Instance.HidePhoneDisplay();
    }
}
