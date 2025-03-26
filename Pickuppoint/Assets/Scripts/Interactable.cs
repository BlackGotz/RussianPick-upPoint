using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;
    public bool IsThrown { get; private set; } = false;

    protected Rigidbody rb; // ���� ��� Rigidbody

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // �������������� Rigidbody ��� ������
    }

    public void BaseInteract()
    {
        Interact();
    }
    public void BaseThrow(Vector3 throwDirection)
    {
        IsThrown = true; // ������������� ���� ��� ������
        Throw(throwDirection); // �������� ��������������� ����� ��� ����������� � Rigidbody
        StartCoroutine(CheckIfLanded()); // ��������� �������� �����������
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
            yield return new WaitForSeconds(0.1f); // ��������� ��� � 0.1 ���

            if (rb != null && rb.velocity.magnitude < 0.1f) // ���� ������ ����� �����������
            {
                IsThrown = false;
                Debug.Log($"{gameObject.name} �����������!");
            }
        }
    }


}
