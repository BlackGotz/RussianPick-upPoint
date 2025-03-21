using UnityEngine;
using TMPro;

public class Client : MonoBehaviour
{
    public int requiredBoxNumber;
    public float moveSpeed = 2f;

    // ����� �������� (� ��������), ������� ����������� ����� �������� � pickupPoint
    public float maxPatience = 10f;
    private float currentPatience;

    // ��������� �������
    private enum ClientState { MovingToPickup, Waiting, Leaving }
    private ClientState state = ClientState.MovingToPickup;

    private Transform pickupPoint;
    private Vector3 spawnPosition; // ����� ������ ������� (��� �����������)

    // ���� ����������� ���������� ����������� ��������, ����� �������� UI-������� (��������, Slider)
    // public Slider patienceSlider;

    private void Start()
    {
        currentPatience = maxPatience;
    }

    private void Update()
    {
        switch (state)
        {
            case ClientState.MovingToPickup:
                MoveTo(pickupPoint.position);
                if (Vector3.Distance(transform.position, pickupPoint.position) < 0.1f)
                {
                    state = ClientState.Waiting;
                    UIManager.Instance.ShowClientPhone(requiredBoxNumber);
                }
                break;

            case ClientState.Waiting:
                // ��������� ��������
                currentPatience -= Time.deltaTime;
                if (currentPatience <= 0f)
                {
                    // �������� ��������� � ����������� ��������� �� Leaving � ���������������
                    PrepareToLeave();
                }
                break;

            case ClientState.Leaving:
                MoveTo(spawnPosition);
                if (Vector3.Distance(transform.position, spawnPosition) < 0.1f)
                {
                    // ������ ������ � �������� ��������� � ���������� ������
                    SpawnManager.Instance.ClientFinished(requiredBoxNumber);
                    Destroy(gameObject);
                }
                break;
        }
    }

    // ������� ����������� � ������� �������
    private void MoveTo(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // ����� ��� ��������� �������
    private void TurnAround()
    {
        Vector3 direction = (spawnPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    // ���������� ������� � ����� (������������ ��� ��� ��������� �������, ��� � ��� ���������� ��������)
    private void PrepareToLeave()
    {
        UIManager.Instance.HidePhoneDisplay();
        TurnAround();
        state = ClientState.Leaving;
    }

    // ���������� �� SpawnManager ��� ��������� ���������� �������
    public void SetupClient(int boxNumber, Transform targetPoint)
    {
        requiredBoxNumber = boxNumber;
        pickupPoint = targetPoint;
        // ��������� ��������� ������� ��� ����� ������, � ������� ������ �������� ��� ���������� ��������
        spawnPosition = transform.position;
    }

    // �����, ���������� ��� ��������� ������� �������
    public void ReceiveBox(Box box)
    {
        if (box.boxNumber == requiredBoxNumber)
        {
            Debug.Log("������� ���������� �������!");
            Destroy(box.gameObject);
            // ��� ��������� ������� ������ ����� ����� ����: ��������������� � ����������� ���������
            PrepareToLeave();
        }
        else
        {
            Debug.Log("�������� �������");
            currentPatience -= maxPatience / 3f;
        }
    }
}
