using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [Header("UI Элементы")]
    [SerializeField] private Image fillImage;

    [Header("Настройки шкалы терпения")]
    // Градиент должен быть настроен так, чтобы:
    // t=1 -> зелёный, t=0.5 -> жёлтый, t=0 -> красный
    [SerializeField] private Gradient colorGradient;

    /// <summary>
    /// Обновляет визуальное отображение шкалы терпения.
    /// </summary>
    /// <param name="currentPatience">Текущее значение терпения.</param>
    /// <param name="maxPatience">Максимальное значение терпения.</param>
    public void UpdateBar(float currentPatience, float maxPatience)
    {
        float t = Mathf.Clamp01(currentPatience / maxPatience);
        fillImage.fillAmount = t;
        fillImage.color = colorGradient.Evaluate(t);
    }
}
