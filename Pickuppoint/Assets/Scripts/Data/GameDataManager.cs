using UnityEngine;
using System.IO;
using System.Collections.Generic;

// Класс для хранения всех игровых данных
[System.Serializable]
public class GameData
{
    // Статистика
    public int currentDay = 1;
    public int balance = 0;
    public float rating = 0f;
    
    // Отзывы
    public List<ReviewData> reviews = new List<ReviewData>();
}

// Класс для хранения данных отзыва
[System.Serializable]
public class ReviewData
{
    public string text;
    public int rating;
    public string clientName;
    public string date;
    
    public ReviewData(string text, int rating, string clientName, string date)
    {
        this.text = text;
        this.rating = rating;
        this.clientName = clientName;
        this.date = date;
    }
}

public class GameDataManager : MonoBehaviour
{
    private static GameDataManager _instance;
    public static GameDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("GameDataManager");
                _instance = obj.AddComponent<GameDataManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }
    
    private GameData gameData = new GameData();
    private string dataPath;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Путь к файлу сохранения
        dataPath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        
        // Загружаем данные при старте
        LoadData();
    }
    
    // Сохранение данных в файл
    public void SaveData()
    {
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(dataPath, json);
        Debug.Log("Данные сохранены в: " + dataPath);
    }
    
    // Загрузка данных из файла
    public void LoadData()
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Данные загружены из: " + dataPath);
        }
        else
        {
            gameData = new GameData();
            Debug.Log("Файл данных не найден. Созданы новые данные.");
            SaveData(); // Создаем новый файл с начальными данными
        }
    }
    
    // Методы для получения данных
    public int GetCurrentDay() { return gameData.currentDay; }
    public int GetBalance() { return gameData.balance; }
    public float GetRating() { return gameData.rating; }
    public List<ReviewData> GetReviews() { return gameData.reviews; }
    
    // Методы для обновления данных
    public void SetCurrentDay(int day)
    {
        gameData.currentDay = day;
        SaveData(); // Сохраняем после изменения
    }
    
    public void SetBalance(int balance)
    {
        gameData.balance = balance;
        SaveData(); // Сохраняем после изменения
    }
    
    public void SetRating(float rating)
    {
        gameData.rating = rating;
        SaveData(); // Сохраняем после изменения
    }
    
    // Метод для добавления отзыва
    public void AddReview(ReviewData review)
    {
        gameData.reviews.Add(review);
        
        // Пересчитываем средний рейтинг
        float sum = 0;
        foreach (var r in gameData.reviews)
        {
            sum += r.rating;
        }
        gameData.rating = gameData.reviews.Count > 0 ? sum / gameData.reviews.Count : 0;
        
        SaveData(); // Сохраняем после изменения
    }
    
    // Метод для очистки всех отзывов
    public void ClearAllReviews()
    {
        gameData.reviews.Clear();
        gameData.rating = 0;
        SaveData(); // Сохраняем после изменения
    }
    
    // Метод для конвертации Review в ReviewData
    public ReviewData ConvertReview(Review review)
    {
        return new ReviewData(review.text, review.rating, review.clientName, review.date);
    }
} 