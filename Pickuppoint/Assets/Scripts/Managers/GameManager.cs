using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject dayEndZone;
    [SerializeField] private GameObject dayEndUI; // UI элемент для завершения дня
    private bool isDayEnding = false;
    private bool isUIActive = false;
    
    [Header("Игровая статистика")]
    [SerializeField] private int moneyPerSatisfiedClient = 100; // Деньги за довольного клиента
    
    // Ссылка на контроллер UI статистики
    private StatisticsUIController statisticsUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Инициализируемся на окончание дня
        TimeManager.Instance.OnDayEnded += EndDay;
        
        // Скрываем зону завершения дня и UI в начале
        if (dayEndZone != null)
            dayEndZone.SetActive(false);
            
        if (dayEndUI != null)
            dayEndUI.SetActive(false);
            
        // Находим контроллер UI статистики
        statisticsUI = FindObjectOfType<StatisticsUIController>();
        
        // Обновляем UI при старте
        UpdateStatisticsUI();
    }
    
    private void Update()
    {
        // Если UI активен, ждем нажатия любой кнопки
        if (isUIActive && Input.anyKeyDown)
        {
            StartNewDay();
        }
    }

    // Вызывается автоматически, когда день заканчивается по времени
    private void EndDay()
    {
        Debug.Log("День окончен");
        ShowDayEndZone();
    }
    
    // Публичный метод для кнопки завершения дня
    public void EndDayButtonPressed()
    {
        Debug.Log("День завершен вручную");
        
        // Можно добавить дополнительные проверки здесь, если нужно
        // Например, проверить, не остались ли клиенты в очереди
        
        // Если хотим показать UI без зоны
        isDayEnding = true;
        ShowDayEndUI();
        
        // Или если хотим показать зону, как при обычном завершении дня:
        // ShowDayEndZone();
    }
    
    private void ShowDayEndZone()
    {
        if (dayEndZone != null)
        {
            dayEndZone.SetActive(true);
            isDayEnding = true;
        }
        else
        {
            Debug.LogWarning("Зона завершения дня не назначена в GameManager!");
        }
    }
    
    public void PlayerEnteredDayEndZone()
    {
        if (isDayEnding)
        {
            ShowDayEndUI();
        }
    }
    
    private void ShowDayEndUI()
    {
        if (dayEndUI != null)
        {
            dayEndUI.SetActive(true);
            isUIActive = true;
        }
        else
        {
            Debug.LogWarning("UI завершения дня не назначен в GameManager!");
            // Если UI не назначен, просто начинаем новый день
            StartNewDay();
        }
    }
    
    private void StartNewDay()
    {
        // Увеличиваем номер дня и сохраняем
        int currentDay = GameDataManager.Instance.GetCurrentDay();
        GameDataManager.Instance.SetCurrentDay(currentDay + 1);
        
        // Обновляем UI перед перезагрузкой сцены
        UpdateStatisticsUI();
        
        // Перезагружаем текущую сцену
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    // Метод вызывается когда клиент обслужен
    public void ClientServed(bool satisfied)
    {
        // Если клиент доволен, увеличиваем баланс
        if (satisfied)
        {
            int balance = GameDataManager.Instance.GetBalance();
            GameDataManager.Instance.SetBalance(balance + moneyPerSatisfiedClient);
            
            // Обновляем UI статистики
            UpdateStatisticsUI();
        }
    }
    
    // Обновление UI статистики
    private void UpdateStatisticsUI()
    {
        if (statisticsUI != null)
        {
            statisticsUI.UpdateUI();
        }
    }
}
