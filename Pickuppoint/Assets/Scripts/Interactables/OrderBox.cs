using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBox : Interactable
{
    private Transform playerHand; // ������ �� ���� ������
    private bool isHeld = false;  // ����, ������ �� ����� �������

    void Start()
    {
        // ������� ������ "Hand" � ����� (������ ���� � ������)
        playerHand = GameObject.Find("Hand").transform;
    }

    protected override void Interact()
    {
        if (!isHeld)
        {
            PickUp();
        }
        else
        {
            Drop();
        }
    }

    private void PickUp()
    {
        // ��������� ������
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // ���������� ������ � ���� ������
        transform.position = playerHand.position;
        transform.rotation = playerHand.rotation;
        transform.SetParent(playerHand);

        isHeld = true;
        Debug.Log("Picked up " + gameObject.name);
    }

    private void Drop()
    {
        // �������� ������ �������
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // ������� ��������, ������ ������ �� �����
        transform.SetParent(null);

        isHeld = false;
        Debug.Log("Dropped " + gameObject.name);
    }
}
