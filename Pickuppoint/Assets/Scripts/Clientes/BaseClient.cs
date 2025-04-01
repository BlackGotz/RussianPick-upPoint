using UnityEngine;
using TMPro;

public abstract class BaseClient : MonoBehaviour
{
    public int requiredBoxNumber;
    public float moveSpeed = 2f;
    public float maxPatience = 10f;
    public float currentPatience;
    Animator animator;
    protected enum ClientState { MovingToPickup, Waiting, Leaving }
    protected ClientState state = ClientState.MovingToPickup;

    protected Transform pickupPoint;
    protected Vector3 spawnPosition;
    protected bool isDelivered;

    [SerializeField] protected PatienceBar patienceBar;
    
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("move", true);
        currentPatience = maxPatience;
        isDelivered = false;
        ToggleRecieveTrigger(false);

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
                    animator.SetBool("move", false);
                    UIManager.Instance.ShowClientPhone(GetDisplayText());

                    patienceBar.gameObject.SetActive(true);
                    patienceBar.UpdateBar(currentPatience, maxPatience);

                    ToggleRecieveTrigger(true); // Включаем триггер
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
    /// Включает или отключает триггер получения коробки
    /// </summary>
    private void ToggleRecieveTrigger(bool isActive)
    {
        RecieveScript trigger = GetComponentInChildren<RecieveScript>();
        if (trigger != null)
        {
            Collider triggerCollider = trigger.GetComponent<Collider>();
            if (triggerCollider != null)
            {
                triggerCollider.enabled = isActive;
                //Debug.Log($"RecieveTrigger {(isActive ? "активирован" : "деактивирован")} для {gameObject.name}");
            }
        }
    }


    /// <summary>
    /// Подготовка клиента к уходу – скрываем телефон, разворачиваем и переводим в состояние Leaving.
    /// </summary>
    protected void PrepareToLeave()
    {
        UIManager.Instance.HidePhoneDisplay();
        if (patienceBar != null)
            patienceBar.gameObject.SetActive(false);

        ToggleRecieveTrigger(false); // Выключаем триггер

        TurnAround();
        animator.SetBool("move", true);
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

    public virtual void InstantLeave()
    {
        if (state == ClientState.MovingToPickup) 
        {
            PrepareToLeave();
        }
        else
        {
            currentPatience = 0f;
        }
    }
}
