using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Спавн клиентов и коробок")]
    public GameObject clientPrefab;
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
        // Устанавливаем время ожидания для спавна клиента
        timeToNextClient = Random.Range(5f, 10f);

        SpawnBoxes();

        // Подписываемся на обновление времени
        TimeManager.Instance.OnTimeUpdated += OnTimeUpdated;
    }

    private void OnTimeUpdated(float currentTime)
    {
        if (!isClientActive)
        {
            // Уменьшаем таймер до появления следующего клиента
            timeToNextClient -= TimeManager.Instance.timeIncrement; // или Time.deltaTime, если обновлять по секундам
            if (timeToNextClient <= 0f && availableBoxNumbers.Count > 0)
            {
                SpawnClient();
            }
        }
    }

    private void SpawnBoxes()
    {
        int boxesCount = Random.Range(minBoxes, maxBoxes + 1);
        Debug.Log("Количество коробок" + boxesCount);
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
        // Выбираем случайный номер коробки для клиента
        int index = Random.Range(0, availableBoxNumbers.Count);
        int requiredNumber = availableBoxNumbers[index];

        // Вычисляем направление спавна клиента, чтобы он смотрел в сторону точки выдачи
        Vector3 direction = (pickupPoint.position - clientSpawnPoint.position).normalized;
        Quaternion spawnRotation = Quaternion.LookRotation(direction);

        GameObject clientObj = Instantiate(clientPrefab, clientSpawnPoint.position, spawnRotation);
        Client client = clientObj.GetComponent<Client>();
        client.SetupClient(requiredNumber, pickupPoint);

        isClientActive = true;
    }

    /// <summary>
    /// Вызывается когда клиент уходит
    /// </summary>
    public void ClientFinished(int deliveredBoxNumber)
    {
        isClientActive = false;
        // Удаляем номер коробки из доступных
        availableBoxNumbers.Remove(deliveredBoxNumber);
        // Генерируем новое время ожидания до появления следующего клиента
        timeToNextClient = Random.Range(5f, 10f);
        UIManager.Instance.HidePhoneDisplay();
    }
}
