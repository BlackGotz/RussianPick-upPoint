using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Спавн клиентов и коробок")]
    public GameObject ordinaryClientPrefab;
    public GameObject hurryClientPrefab;
    public GameObject mysteriousClientPrefab;
    public Transform clientSpawnPoint;
    public Transform pickupPoint;
    public GameObject boxPrefab;
    public Transform boxSpawnPoint;

    [Header("Настройки коробок")]
    public int minBoxes = 3;
    public int maxBoxes = 7;
    public float boxesSpawnDeadline = 17 * 60f; // до 17:00

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
        timeToNextClient = Random.Range(5f, 10f);
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
        int boxesCount = Random.Range(minBoxes, maxBoxes + 1);
        Debug.Log("Количество коробок: " + boxesCount);
        for (int i = 0; i < boxesCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * 1f;
            Vector3 spawnPos = boxSpawnPoint.position + new Vector3(randomCircle.x, 0, randomCircle.y);
            GameObject boxObj = Instantiate(boxPrefab, spawnPos, Quaternion.identity);
            int boxNumber = Random.Range(10000, 99999);
            Box box = boxObj.GetComponent<Box>();
            box.SetupBox(boxNumber);
            availableBoxNumbers.Add(boxNumber);
        }
    }

    private void SpawnClient()
    {
        int index = Random.Range(0, availableBoxNumbers.Count);
        int requiredNumber = availableBoxNumbers[index];

        Vector3 direction = (pickupPoint.position - clientSpawnPoint.position).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(direction);

        // Случайно выбираем тип клиента: 0 – обычный, 1 – спешащий, 2 – загадочный
        int clientType = Random.Range(0, 3);
        GameObject clientObj = null;
        switch (clientType)
        {
            case 0:
                clientObj = Instantiate(ordinaryClientPrefab, clientSpawnPoint.position, spawnRotation);
                Debug.Log("Оригинальный клиент");
                break;
            case 1:
                clientObj = Instantiate(hurryClientPrefab, clientSpawnPoint.position, spawnRotation);
                Debug.Log("Спешащий клиен");
                break;
            case 2:
                clientObj = Instantiate(mysteriousClientPrefab, clientSpawnPoint.position, spawnRotation);
                Debug.Log("Загадочный мудак");
                break;
        }

        BaseClient client = clientObj.GetComponent<BaseClient>();
        client.SetupClient(requiredNumber, pickupPoint);
        isClientActive = true;
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
        timeToNextClient = Random.Range(5f, 10f);
        UIManager.Instance.HidePhoneDisplay();
    }
}
