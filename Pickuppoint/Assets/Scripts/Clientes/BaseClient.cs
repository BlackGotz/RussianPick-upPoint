using UnityEngine;
using TMPro;

public abstract class BaseClient : MonoBehaviour
{
    public int requiredBoxNumber;
    public float moveSpeed = 2f;
    public float maxPatience = 10f;
    protected float currentPatience;

    protected enum ClientState { MovingToPickup, Waiting, Leaving }
    protected ClientState state = ClientState.MovingToPickup;

    protected Transform pickupPoint;
    protected Vector3 spawnPosition;
    protected bool isDelivered;

    [SerializeField] protected PatienceBar patienceBar;

    protected virtual void Start()
    {
        currentPatience = maxPatience;
        isDelivered = false;

        if (patienceBar != null)
            patienceBar.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        switch (state)
        {
            case ClientState.MovingToPickup:
                MoveTo(pickupPoint.position);
                if (Vector3.Distance(transform.position, pickupPoint.position) < 0.1f)
                {
                    state = ClientState.Waiting;
                    UIManager.Instance.ShowClientPhone(GetDisplayText());

                    patienceBar.gameObject.SetActive(true);
                    patienceBar.UpdateBar(currentPatience, maxPatience);
                }
                break;

            case ClientState.Waiting:
                currentPatience -= Time.deltaTime;

                if (currentPatience <= 0f)
                {
                    PrepareToLeave();
                }
                else 
                    patienceBar.UpdateBar(currentPatience, maxPatience);

                break;

            case ClientState.Leaving:
                MoveTo(spawnPosition);
                if (Vector3.Distance(transform.position, spawnPosition) < 0.1f)
                {
                    // Клиент уходит – сообщаем менеджеру и уничтожаем объект
                    SpawnManager.Instance.ClientFinished(requiredBoxNumber, isDelivered);
                    Destroy(gameObject);
                }
                break;
        }
    }

    protected void MoveTo(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    protected void TurnAround()
    {
        Vector3 direction = (spawnPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>
    /// Подготовка клиента к уходу – скрываем телефон, разворачиваем и переводим в состояние Leaving.
    /// </summary>
    protected void PrepareToLeave()
    {
        UIManager.Instance.HidePhoneDisplay();
        if (patienceBar != null)
            patienceBar.gameObject.SetActive(false);
        TurnAround();
        state = ClientState.Leaving;
    }

    /// <summary>
    /// Каждый тип клиента может переопределять, что отображать на телефоне.
    /// </summary>
    protected virtual string GetDisplayText()
    {
        return requiredBoxNumber.ToString();
    }

    public virtual void SetupClient(int boxNumber, Transform targetPoint)
    {
        requiredBoxNumber = boxNumber;
        pickupPoint = targetPoint;
        spawnPosition = transform.position;
    }

    public virtual void ReceiveBox(Box box)
    {
        if (box.boxNumber == requiredBoxNumber)
        {
            Debug.Log("Коробка доставлена успешно!");
            Destroy(box.gameObject);
            isDelivered = true;
            // При получении коробки клиент сразу готов уйти
            PrepareToLeave();
        }
        else
        {
            Debug.Log("Неверная коробка");
            currentPatience -= maxPatience / 3f;
        }
    }
}
