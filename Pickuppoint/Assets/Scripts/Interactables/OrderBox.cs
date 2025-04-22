using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderBox : Interactable
{
    private Transform playerHand; // ������ �� ���� ������
    PlayerHand phCondition;
    private bool isHeld = false;  // ����, ������ �� ����� �������
    public float throwForce = 10f; // ���� ������
    public float pickUpSpeed = 1500f; // �������� �������� ��������
    public float maxPickUpDistance = 2f; // ������������ ���������� ��������
    [SerializeField] private LayerMask collisionLayers; // ���� ��� �������� ��������

    private Vector3 targetPosition;
    private bool isMovingToHand = false;

    void Start()
    {
        // ������� ������ "Hand" � ����� (������ ���� � ������)
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
        // ��������� ������
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // �������� �������� � ����
        isMovingToHand = true;
        isHeld = true;
        phCondition.IsBusy = true;
        Debug.Log("Starting to pick up " + gameObject.name);
    }

    private void MoveToHand()
    {
        // �������� ����������� � ����
        Vector3 direction = playerHand.position - transform.position;
        float distance = direction.magnitude;

        if (distance > 2f)
            Drop();

        // ��������� �������� �� ����
        RaycastHit hit;
        bool hasHit = Physics.Raycast(transform.position, direction.normalized, out hit, distance, collisionLayers);

        if (hasHit)
        {
            // ���� ���� �����������, ������������ � ����� ����� ������������
            targetPosition = hit.point - direction.normalized * 0.5f; // ��������� ������
        }
        else
        {
            // ���� ����������� ���, ������������ � ����
            targetPosition = playerHand.position;
        }

        // ������������� �������
        transform.position = Vector3.Lerp(transform.position, targetPosition, pickUpSpeed);
    }

    private void Drop()
    {
        // �������� ������ �������
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // ������� ��������, ������ ������ �� �����
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

                // ������ ������ � ����������� ������� ������
                rb.AddForce(direction * throwForce, ForceMode.VelocityChange);
                InvertFlagThrow();
            }

            // ������� �������� ����� ������
            transform.SetParent(null);
            isHeld = false;
            isMovingToHand = false;
            phCondition.IsBusy = false;
            Debug.Log("Threw " + gameObject.name);
        }
    }
}