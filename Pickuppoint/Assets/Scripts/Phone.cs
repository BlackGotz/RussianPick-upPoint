using System.Collections;
using UnityEngine;
using TMPro;

public class Phone : MonoBehaviour
{
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private float displayTime = 5f;
    [SerializeField] private float hideTime = 3f;

    private MeshRenderer phoneMesh;

    string code;

    private void Start()
    {
        phoneMesh = GetComponent<MeshRenderer>();
        StartCoroutine(PhoneLoop());
    }

    private IEnumerator PhoneLoop()
    {
        while (true)
        {

            code = GenerateCode();
            DisplayCode(code);

            ShowPhone();

            yield return new WaitForSeconds(displayTime);

            HidePhone();

            yield return new WaitForSeconds(hideTime);
        }
    }

    // Показываем телефон
    public void ShowPhone()
    {
        phoneMesh.enabled = true;
    }

    // Скрываем телефон
    public void HidePhone()
    {
        phoneMesh.enabled = false;
    }

    // Генирация 6-ти значного кода
    public string GenerateCode()
    {
        return UnityEngine.Random.Range(100000, 999999).ToString();
    }

    // Отображение кода
    public void DisplayCode(string code)
    {
        codeText.text = code;
    }
}
