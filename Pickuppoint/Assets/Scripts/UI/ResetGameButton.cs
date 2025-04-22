using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class ResetGameButton : MonoBehaviour
{
    private Button button;
    
    [Tooltip("Сцена для загрузки после сброса данных (оставьте пустым для перезагрузки текущей сцены)")]
    [SerializeField] private string sceneToLoad = "";
    
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }
    
    private void OnButtonClick()
    {
        // Сбрасываем все сохраненные данные
        ResetGameData();
        
        // Загружаем указанную сцену или перезагружаем текущую
        string sceneToLoadName = string.IsNullOrEmpty(sceneToLoad) ? 
            SceneManager.GetActiveScene().name : sceneToLoad;
            
        SceneManager.LoadScene(sceneToLoadName);
    }
    
    private void ResetGameData()
    {
        Debug.Log("Сброс данных игры");
        
        // Создаем новые данные через GameDataManager
        GameDataManager gameDataManager = GameDataManager.Instance;
        
        // Сбрасываем параметры на значения по умолчанию
        gameDataManager.SetCurrentDay(1);
        gameDataManager.SetBalance(0);
        gameDataManager.SetRating(0);
        
        // Очищаем все отзывы
        gameDataManager.ClearAllReviews();
    }
    
    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
} 