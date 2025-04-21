using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Спавн клиентов и коробок")]
    public GameObject[] clientModels; // Все возможные модели клиентов
    public Transform clientSpawnPoint;
    public Transform pickupPoint;
    public GameObject boxPrefab;
    public Transform boxSpawnPoint;

    [Header("Настройки коробок")]
    public int minBoxes = 5;
    public int maxBoxes = 9;
    public float boxesSpawnDeadline = 17 * 60f; // до 17:00

    private List<int> availableBoxNumbers = new List<int>();
    private bool isClientActive = false;
    private float timeToNextClient;

    private Type[] clientTypes = new Type[] { typeof(OrdinaryClient), typeof(HurryClient), typeof(MysteriousClient) };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        timeToNextClient = (float)UnityEngine.Random.Range(5, 20);
        SpawnBoxes();
        TimeManager.Instance.OnTimeUpdated += OnTimeUpdated;
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
        Debug.Log("Количество коробок: " + boxesCount);
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

    private void SpawnClient()
    {
        int index = UnityEngine.Random.Range(0, availableBoxNumbers.Count);
        int requiredNumber = availableBoxNumbers[index];

        Vector3 direction = (pickupPoint.position - clientSpawnPoint.position).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(direction);

        // Выбираем модель клиента
        GameObject selectedModel = clientModels[UnityEngine.Random.Range(0, 10) % 2];
        GameObject clientObj = Instantiate(selectedModel, clientSpawnPoint.position, spawnRotation);

        // Выбираем тип поведения клиента
        Type clientType;

        int rand = UnityEngine.Random.Range(0, 100);
        if (rand < 50) // 0–49
        {
            clientType = typeof(OrdinaryClient);
        }
        else if (rand < 80) // 50–79
        {
            clientType = typeof(HurryClient);
        }
        else // 80–99
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
    /// Вызывается, когда клиент уходит (либо успешно получил коробку, либо ушёл из-за терпения)
    /// </summary>
    public void ClientFinished(int deliveredBoxNumber, bool isDelivered)
    {
        isClientActive = false;
        if (isDelivered)
        {
            availableBoxNumbers.Remove(deliveredBoxNumber);
        }
        timeToNextClient = (float)UnityEngine.Random.Range(5, 20);
        Debug.Log("Время до клиента" + timeToNextClient);
        //UIManager.Instance.HidePhoneDisplay();
    }
}
