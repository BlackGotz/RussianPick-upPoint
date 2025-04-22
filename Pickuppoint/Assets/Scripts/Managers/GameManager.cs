using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private GameObject dayEndZone;
    [SerializeField] private GameObject dayEndUI; // UI элемент для завершения дня
    private bool isDayEnding = false;
    private bool isUIActive = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //    
        TimeManager.Instance.OnDayEnded += EndDay;
        
        // Скрываем зону завершения дня и UI в начале
        if (dayEndZone != null)
            dayEndZone.SetActive(false);
            
        if (dayEndUI != null)
            dayEndUI.SetActive(false);
    }
    
    private void Update()
    {
        // Если UI активен, ждем нажатия любой кнопки
        if (isUIActive && Input.anyKeyDown)
        {
            StartNewDay();
        }
    }

    private void EndDay()
    {
        Debug.Log(" ");
        //       
        ShowDayEndZone();
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
        // Перезагружаем текущую сцену
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
