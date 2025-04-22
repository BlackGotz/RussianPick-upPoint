using UnityEngine;

// Этот компонент нужно добавить на объект в начальной сцене
public class GameDataInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Инициализируем GameDataManager при запуске игры
        // Вызов Instance создаст экземпляр, если его еще нет
        GameDataManager dataManager = GameDataManager.Instance;
        Debug.Log("GameDataManager был инициализирован");
    }
} 