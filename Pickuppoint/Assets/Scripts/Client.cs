using UnityEngine;
using TMPro;

public class Client : MonoBehaviour
{
    public int requiredBoxNumber;
    public float moveSpeed = 2f;

    // Время терпения (в секундах), которое расходуется после прибытия в pickupPoint
    public float maxPatience = 10f;
    private float currentPatience;

    // Состояния клиента
    private enum ClientState { MovingToPickup, Waiting, Leaving }
    private ClientState state = ClientState.MovingToPickup;

    private Transform pickupPoint;
    private Vector3 spawnPosition; // Точка спавна клиента (для возвращения)

    // Если потребуется визуальное отображение терпения, можно добавить UI-элемент (например, Slider)
    // public Slider patienceSlider;

    private void Start()
    {
        currentPatience = maxPatience;
    }

    private void Update()
    {
        switch (state)
        {
            case ClientState.MovingToPickup:
                MoveTo(pickupPoint.position);
                if (Vector3.Distance(transform.position, pickupPoint.position) < 0.1f)
                {
                    state = ClientState.Waiting;
                    UIManager.Instance.ShowClientPhone(requiredBoxNumber);
                }
                break;

            case ClientState.Waiting:
                // Расходуем терпение
                currentPatience -= Time.deltaTime;
                if (currentPatience <= 0f)
                {
                    // Терпение исчерпано — переключаем состояние на Leaving и разворачиваемся
                    PrepareToLeave();
                }
                break;

            case ClientState.Leaving:
                MoveTo(spawnPosition);
                if (Vector3.Distance(transform.position, spawnPosition) < 0.1f)
                {
                    // Клиент уходит — сообщаем менеджеру и уничтожаем объект
                    SpawnManager.Instance.ClientFinished(requiredBoxNumber);
                    Destroy(gameObject);
                }
                break;
        }
    }

    // Плавное перемещение к целевой позиции
    private void MoveTo(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // Метод для разворота клиента
    private void TurnAround()
    {
        Vector3 direction = (spawnPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // Подготовка клиента к уходу (используется как при получении коробки, так и при исчерпании терпения)
    private void PrepareToLeave()
    {
        UIManager.Instance.HidePhoneDisplay();
        TurnAround();
        state = ClientState.Leaving;
    }

    // Вызывается из SpawnManager для установки параметров клиента
    public void SetupClient(int boxNumber, Transform targetPoint)
    {
        requiredBoxNumber = boxNumber;
        pickupPoint = targetPoint;
        // Сохраняем начальную позицию как точку спавна, к которой клиент вернется при исчерпании терпения
        spawnPosition = transform.position;
    }

    // Метод, вызываемый при получении коробки игроком
    public void ReceiveBox(Box box)
    {
        if (box.boxNumber == requiredBoxNumber)
        {
            Debug.Log("Коробка доставлена успешно!");
            Destroy(box.gameObject);
            // При получении коробки клиент сразу готов уйти: разворачиваемся и переключаем состояние
            PrepareToLeave();
        }
        else
        {
            Debug.Log("Неверная коробка");
            currentPatience -= maxPatience / 3f;
        }
    }
}
