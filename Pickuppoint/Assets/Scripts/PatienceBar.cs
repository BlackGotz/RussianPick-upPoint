using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [Header("UI ��������")]
    [SerializeField] private Image fillImage;

    [Header("��������� ����� ��������")]
    // �������� ������ ���� �������� ���, �����:
    // t=1 -> ������, t=0.5 -> �����, t=0 -> �������
    [SerializeField] private Gradient colorGradient;

    /// <summary>
    /// ��������� ���������� ����������� ����� ��������.
    /// </summary>
    /// <param name="currentPatience">������� �������� ��������.</param>
    /// <param name="maxPatience">������������ �������� ��������.</param>
    public void UpdateBar(float currentPatience, float maxPatience)
    {
        float t = Mathf.Clamp01(currentPatience / maxPatience);
        fillImage.fillAmount = t;
        fillImage.color = colorGradient.Evaluate(t);
    }
}
