using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EndDayButton : MonoBehaviour
{
    private Button button;
    
    private void Awake()
    {
        button = GetComponent<Button>();
        
        // Добавляем слушатель на нажатие кнопки
        button.onClick.AddListener(OnButtonClick);
    }
    
    private void OnButtonClick()
    {
        // Вызываем метод завершения дня у GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndDayButtonPressed();
        }
        else
        {
            Debug.LogError("GameManager.Instance не найден!");
        }
    }
    
    private void OnDestroy()
    {
        // Удаляем слушатель при уничтожении компонента
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
} 