using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    public int boxNumber;
    private Text boxNumberDisplay;

    private void Awake()
    {
        // ���� ������ �� ������ ����� ���������, ���������� ����� Text ����� �������� ��������.
        if (boxNumberDisplay == null)
        {
            boxNumberDisplay = GetComponentInChildren<Text>();
        }
    }

    public void SetupBox(int number)
    {
        boxNumber = number;
        if (boxNumberDisplay != null)
            boxNumberDisplay.text = boxNumber.ToString();
    }
}
