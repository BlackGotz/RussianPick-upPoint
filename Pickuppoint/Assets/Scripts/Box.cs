using UnityEngine;
using TMPro;

public class Box : MonoBehaviour
{
    public int boxNumber;
    [SerializeField] private TextMeshPro[] boxNumberDisplays; // массив всех надписей на коробке

    private void Awake()
    {
        // Если массив не задан в инспекторе — попытаемся найти все TextMeshPro среди детей
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
