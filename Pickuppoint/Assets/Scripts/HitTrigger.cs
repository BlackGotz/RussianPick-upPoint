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
            Debug.LogError($"������: BaseClient �� ������ � ������������ �������� {gameObject.name}!");
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
            Debug.Log("������� ����� �������, ������� ��������");
        }
    }
}
