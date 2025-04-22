using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("����� �������� � �������")]
    public GameObject[] clientModels; // ��� ��������� ������ ��������
    public Transform clientSpawnPoint;
    public Transform pickupPoint;
    public GameObject boxPrefab;
    public Transform boxSpawnPoint;
    public BoardShelf[] boardShelves;

    public List<Transform> boxSpawnPoints = new List<Transform>();

    [Header("��������� �������")]
    public int minBoxes = 5;
    public int maxBoxes = 9;
    public float boxesSpawnDeadline = 17 * 60f; // �� 17:00

    private List<int> availableBoxNumbers = new List<int>();
    private bool isClientActive = false;
    private float timeToNextClient;

    private Type[] clientTypes = new Type[] { typeof(OrdinaryClient), typeof(HurryClient), typeof(MysteriousClient) };
    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CollectAllSpawnPoints();
        timeToNextClient = (float)UnityEngine.Random.Range(5, 20);
        SpawnBoxesMorning();
        TimeManager.Instance.OnTimeUpdated += OnTimeUpdated;
    }

    void CollectAllSpawnPoints()
    {
        boxSpawnPoints.Clear(); // �� ������ ������

        foreach (BoardShelf shelf in boardShelves)
        {
            if (shelf != null && shelf.spawnPoints != null)
                boxSpawnPoints.AddRange(shelf.spawnPoints);
        }

        Debug.Log($"���������� ����� ������: {boxSpawnPoints.Count}");
    }

    private void OnTimeUpdated(float currentTime)
    {
        if (!isClientActive)
        {
            timeToNextClient -= TimeManager.Instance.timeIncrement;
            if (timeToNextClient <= 0f && availableBoxNumbers.Count > 0)
            {
                SpawnClient();
            }
        }
    }

    private void SpawnBoxes()
    {
        int boxesCount = UnityEngine.Random.Range(minBoxes, maxBoxes + 1);
        Debug.Log("���������� �������: " + boxesCount);
        for (int i = 0; i < boxesCount; i++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 1f;
            Vector3 spawnPos = boxSpawnPoint.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            GameObject boxObj = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
            int boxNumber = UnityEngine.Random.Range(10000, 99999);
            Box box = boxObj.GetComponent<Box>();
            box.SetupBox(boxNumber);
            availableBoxNumbers.Add(boxNumber);
        }
    }

    private void SpawnBoxesMorning()
    {
        int boxesCount = UnityEngine.Random.Range(minBoxes, maxBoxes + 1);
        Debug.Log("���������� �������: " + boxesCount);
        Shuffle(boxSpawnPoints);

        for (int i = 0; i < boxesCount; i++)
        {
            Vector3 spawnPos = boxSpawnPoints[i].position;
            GameObject boxObj = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
            int boxNumber = UnityEngine.Random.Range(10000, 99999);
            Box box = boxObj.GetComponent<Box>();
            box.SetupBox(boxNumber);
            availableBoxNumbers.Add(boxNumber);
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random(); 

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void SpawnClient()
    {
        int index = UnityEngine.Random.Range(0, availableBoxNumbers.Count);
        int requiredNumber = availableBoxNumbers[index];

        Vector3 direction = (pickupPoint.position - clientSpawnPoint.position).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(direction);

        // �������� ������ �������
        GameObject selectedModel = clientModels[UnityEngine.Random.Range(0, 10) % 2];
        GameObject clientObj = Instantiate(selectedModel, clientSpawnPoint.position, spawnRotation);

        // �������� ��� ��������� �������
        Type clientType;

        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 60) // 0�49
        {
            clientType = typeof(OrdinaryClient);
        }
        else if (rand < 90) // 50�89
        {
            clientType = typeof(HurryClient);
        }
        else // 80�99
        {
            clientType = typeof(MysteriousClient);
        }

        BaseClient client = (BaseClient)clientObj.AddComponent(clientType);

        client.SetupClient(requiredNumber, pickupPoint);
        isClientActive = true;

        Debug.Log("clientType " + clientType);
        Debug.Log("requiredNumber " + requiredNumber);
    }

    /// <summary>
    /// ����������, ����� ������ ������ (���� ������� ������� �������, ���� ���� ��-�� ��������)
    /// </summary>
    public void ClientFinished(int deliveredBoxNumber, bool isDelivered)
    {
        isClientActive = false;
        if (isDelivered)
        {
            availableBoxNumbers.Remove(deliveredBoxNumber);
        }
        timeToNextClient = (float)UnityEngine.Random.Range(5, 20);
        Debug.Log("����� �� �������" + timeToNextClient);
        //UIManager.Instance.HidePhoneDisplay();
    }
}
