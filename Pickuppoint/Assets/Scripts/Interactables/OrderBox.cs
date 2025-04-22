using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBox : Interactable
{
    private Transform playerHand; // Ссылка на руку игрока
    PlayerHand phCondition;
    private bool isHeld = false;  // Флаг, держит ли игрок предмет
    public float throwForce = 10f; // Сила броска
    public float pickUpSpeed = 1500f; // Скорость поднятия предмета
    public float maxPickUpDistance = 2f; // Максимальное расстояние поднятия
    [SerializeField] private LayerMask collisionLayers; // Слои для проверки коллизий

    private Vector3 targetPosition;
    private bool isMovingToHand = false;

    void Start()
    {
        // Находим объект "Hand" в сцене (должен быть у игрока)
        playerHand = GameObject.Find("Hand").transform;
        phCondition = playerHand.GetComponent<PlayerHand>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isMovingToHand)
        {
            MoveToHand();
        }
    }

    protected override void Interact()
    {
        if (!isHeld)
        {
            if (!phCondition.IsBusy)
            {
                StartPickUp();
            }
        }
        else
        {
            Drop();
        }
    }

    private void StartPickUp()
    {
        // Отключаем физику
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Начинаем движение к руке
        isMovingToHand = true;
        isHeld = true;
        phCondition.IsBusy = true;
        Debug.Log("Starting to pick up " + gameObject.name);
    }

    private void MoveToHand()
    {
        // Получаем направление к руке
        Vector3 direction = playerHand.position - transform.position;
        float distance = direction.magnitude;

        if (distance > 2f)
            Drop();

        // Проверяем коллизии по пути
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, direction.normalized, out hit, distance, collisionLayers);

        if (hasHit)
        {
            // Если есть препятствие, перемещаемся к точке перед препятствием
            targetPosition = hit.point - direction.normalized * 0.5f; // небольшой отступ
        }
        else
        {
            // Если препятствий нет, перемещаемся к руке
            targetPosition = playerHand.position;
        }

        // Интерполируем позицию
        transform.position = Vector3.Lerp(transform.position, targetPosition, pickUpSpeed);
    }

    private void Drop()
    {
        // Включаем физику обратно
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Убираем родителя, объект падает на землю
        transform.SetParent(null);

        isHeld = false;
        isMovingToHand = false;
        phCondition.IsBusy = false;
        Debug.Log("Dropped " + gameObject.name);
    }

    protected override void Throw(Vector3 direction)
    {
        if (isHeld)
        {
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;

                // Задаем бросок в направлении взгляда игрока
                rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
                InvertFlagThrow();
            }

            // Убираем родителя после броска
            transform.SetParent(null);
            isHeld = false;
            isMovingToHand = false;
            phCondition.IsBusy = false;
            Debug.Log("Threw " + gameObject.name);
        }
    }
}