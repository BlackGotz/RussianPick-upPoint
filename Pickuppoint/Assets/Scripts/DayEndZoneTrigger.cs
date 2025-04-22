using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DayEndZoneTrigger : MonoBehaviour
{
    [SerializeField] private Color activeColor = Color.green; // Цвет при активации
    [SerializeField] private float pulseSpeed = 1.0f; // Скорость пульсации
    [SerializeField] private float minIntensity = 0.3f; // Минимальная интенсивность свечения
    
    private Renderer rend;
    private Color originalColor;
    private bool isActive = false;
    
    private void Awake()
    {
        // Убедимся, что у объекта активирован триггер
        Collider collider = GetComponent<Collider>();
        if (collider != null && !collider.isTrigger)
        {
            collider.isTrigger = true;
            Debug.Log("Автоматически установлен isTrigger = true для коллайдера зоны завершения дня");
        }
        
        // Получаем компонент рендерера если есть
        rend = GetComponent<Renderer>();
        if (rend != null) 
        {
            originalColor = rend.material.color;
        }
    }
    
    private void OnEnable()
    {
        // Визуально показываем, что зона активна
        isActive = true;
    }
    
    private void OnDisable()
    {
        isActive = false;
        // Возвращаем оригинальный цвет при деактивации
        if (rend != null)
        {
            rend.material.color = originalColor;
        }
    }
    
    private void Update()
    {
        // Создаем пульсирующий эффект для визуального индикатора
        if (isActive && rend != null)
        {
            float pulse = minIntensity + Mathf.PingPong(Time.time * pulseSpeed, 1.0f - minIntensity);
            rend.material.color = activeColor * pulse;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, является ли объект игроком
        if (other.CompareTag("Player"))
        {
            // Сообщаем GameManager, что игрок вошел в зону завершения дня
            GameManager.Instance.PlayerEnteredDayEndZone();
        }
    }
} 