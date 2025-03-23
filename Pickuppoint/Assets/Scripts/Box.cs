using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Box : MonoBehaviour
{
    public int boxNumber;
    private TextMeshPro boxNumberDisplay;

    private void Awake()
    {
        // Если ссылка не задана через инспектор, попытаемся найти Text среди дочерних объектов.
        if (boxNumberDisplay == null)
        {
            boxNumberDisplay = GetComponentInChildren<TextMeshPro>();
        }
    }

    public void SetupBox(int number)
    {
        boxNumber = number;
        if (boxNumberDisplay != null)
            boxNumberDisplay.text = boxNumber.ToString();
    }
}
