using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    public bool IsThrown { get; private set; } = false;

    protected Rigidbody rb; // Поле для Rigidbody

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Инициализируем Rigidbody при старте
    }

    public void BaseInteract()
    {
        Interact();
    }
    public void BaseThrow(Vector3 throwDirection)
    {
        IsThrown = true; // Устанавливаем флаг при броске
        Throw(throwDirection); // Вызываем переопределённый метод для манипуляций с Rigidbody
        StartCoroutine(CheckIfLanded()); // Запускаем проверку приземления
    }


    protected virtual void Interact()
    {

    }

    protected virtual void Throw(Vector3 direction)
    {

    }
    public bool GetIsThrown()
    {
        return IsThrown;
    }

    private IEnumerator CheckIfLanded()
    {
        while (IsThrown)
        {
            yield return new WaitForSeconds(0.1f); // Проверяем раз в 0.1 сек

            if (rb != null && rb.velocity.magnitude < 0.1f) // Если объект почти остановился
            {
                IsThrown = false;
                Debug.Log($"{gameObject.name} приземлился!");
            }
        }
    }


}
