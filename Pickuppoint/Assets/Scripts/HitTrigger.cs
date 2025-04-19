using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BaseClient;

public class HitTrigger : MonoBehaviour
{
    private BaseClient currentClient;

    private void Start()
    {
        currentClient = GetComponentInParent<BaseClient>();

        if (currentClient == null)
        {
            Debug.LogError($"Ошибка: BaseClient не найден в родительских объектах {gameObject.name}!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable item = other.GetComponent<Interactable>();

        if (item == null)
            return;

        if (item.GetIsThrown())
        {
            currentClient.InstantLeave();
            Debug.Log("Предмет задел клиента, обнуляю терпение");
        }
    }
}
