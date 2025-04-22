using UnityEngine;
using TMPro;

public class Box : MonoBehaviour
{
    public int boxNumber;
    [SerializeField] private TextMeshPro[] boxNumberDisplays; // ������ ���� �������� �� �������

    private void Awake()
    {
        // ���� ������ �� ����� � ���������� � ���������� ����� ��� TextMeshPro ����� �����
        if (boxNumberDisplays == null || boxNumberDisplays.Length == 0)
        {
            boxNumberDisplays = GetComponentsInChildren<TextMeshPro>();
        }
    }

    public void SetupBox(int number)
    {
        boxNumber = number;

        foreach (var display in boxNumberDisplays)
        {
            if (display != null)
                display.text = boxNumber.ToString();
        }
    }
}
