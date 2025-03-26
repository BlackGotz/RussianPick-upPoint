using UnityEngine;

public class RecieveScript : MonoBehaviour
{
    private BaseClient currentClient;

    private void Start()
    {
        // ���� ���������-��������� BaseClient ���� ��� ��� ������
        currentClient = GetComponentInParent<BaseClient>();

        if (currentClient == null)
        {
            Debug.LogError($"������: BaseClient �� ������ � ������������ �������� {gameObject.name}!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Box box = other.GetComponent<Box>(); // ���������, �������� �� �������� ������ ��������
        Interactable item = other.GetComponent<Interactable>();
        if (item.GetIsThrown())
            {
                Debug.Log("������� ��� ������!");
            }
        if (box != null)
        {
            if (currentClient != null)
            {
                currentClient.ReceiveBox(box);
                //Debug.Log($"Box {box.name} ������� ������� {currentClient.name}");
            }
            else
            {
                Debug.LogError("������: ������� ������ (BaseClient) �� ������!");
            }
        }
    }
}
