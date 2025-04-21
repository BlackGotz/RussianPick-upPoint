using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBox : Interactable
{
    private Transform playerHand; // Ссылка на руку игрока
    private bool isHeld = false;  // Флаг, держит ли игрок предмет
    public float throwForce = 10f; // Сила броска

    

    void Start()
    {
        // Находим объект "Hand" в сцене (должен быть у игрока)
        playerHand = GameObject.Find("Hand").transform;
    }

    protected override void Interact()
    {
        if (!isHeld)
        {
            if (playerHand.childCount == 0)
            {
                PickUp();
            }
        }
        else
        {
            Drop();
        }
    }

    private void PickUp()
    {
        // Отключаем физику
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Перемещаем объект в руку игрока
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        transform.SetParent(playerHand);

        isHeld = true;
        Debug.Log("Picked up " + gameObject.name);
    }

    private void Drop()
    {
        // Включаем физику обратно
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Убираем родителя, объект падает на землю
        transform.SetParent(null);

        isHeld = false;
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
            Debug.Log("Threw " + gameObject.name);
        }
    }
}
