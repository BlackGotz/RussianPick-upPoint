using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Review
{
    public string text;
    public int rating; // от 1 до 5
    public string clientName;
    public string date;

    public Review(string text, int rating, string clientName, string date)
    {
        this.text = text;
        this.rating = rating;
        this.clientName = clientName;
        this.date = date;
    }
}

public class ReviewSystem : MonoBehaviour
{
    public static ReviewSystem Instance;

    [SerializeField] private GameObject reviewPrefab;
    [SerializeField] private Transform reviewsContainer; // ScrollView content
    
    private List<Review> reviews = new List<Review>();
    
    [SerializeField] private string[] positiveComments = new string[] 
    {
        "Отличный сервис, очень быстро получил посылку!",
        "Все прошло гладко, спасибо за обслуживание!",
        "Персонал вежливый, быстро нашли мою посылку.",
        "Приятно удивлен качеством обслуживания!",
        "Лучший пункт выдачи в городе!"
    };
    
    [SerializeField] private string[] neutralComments = new string[] 
    {
        "Обычный сервис, ничего особенного.",
        "Получил посылку без проблем, но долго ждал.",
        "Нормально, но есть куда расти.",
        "Сотрудники могли быть повежливее.",
        "В целом неплохо, но долго искали мою посылку."
    };
    
    [SerializeField] private string[] negativeComments = new string[] 
    {
        "Ужасное обслуживание, долго ждал!",
        "Сотрудники грубые, никогда больше сюда не приду!",
        "Потеряли мою посылку, еле нашли!",
        "Очень недоволен качеством обслуживания.",
        "Худший пункт выдачи из всех, что я посещал."
    };
    
    [SerializeField] private string[] clientNames = new string[] 
    {
        "Александр", "Екатерина", "Дмитрий", "Ольга", "Михаил", 
        "Анна", "Сергей", "Наталья", "Иван", "Мария"
    };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {
        // Загружаем отзывы из GameDataManager
        LoadReviewsFromSave();
        DisplayAllReviews();
    }
    
    // Загрузка сохраненных отзывов
    private void LoadReviewsFromSave()
    {
        reviews.Clear();
        
        // Получаем сохраненные отзывы из GameDataManager
        List<ReviewData> savedReviews = GameDataManager.Instance.GetReviews();
        
        // Конвертируем ReviewData в Review
        foreach (ReviewData reviewData in savedReviews)
        {
            Review review = new Review(
                reviewData.text, 
                reviewData.rating, 
                reviewData.clientName, 
                reviewData.date
            );
            reviews.Add(review);
        }
    }
    
    // Создать отзыв на основе терпения клиента
    public void CreateReviewFromPatience(float patiencePercent)
    {
        int rating;
        string comment;
        
        // Определяем рейтинг и комментарий на основе терпения
        if (patiencePercent >= 0.7f) // Высокое терпение - довольный клиент
        {
            rating = Random.Range(4, 6); // 4-5 звезд
            comment = positiveComments[Random.Range(0, positiveComments.Length)];
        }
        else if (patiencePercent >= 0.4f) // Среднее терпение
        {
            rating = Random.Range(3, 5); // 3-4 звезды
            comment = neutralComments[Random.Range(0, neutralComments.Length)];
        }
        else // Низкое терпение - недовольный клиент
        {
            rating = Random.Range(1, 3); // 1-2 звезды
            comment = negativeComments[Random.Range(0, negativeComments.Length)];
        }
        
        string clientName = clientNames[Random.Range(0, clientNames.Length)];
        string date = System.DateTime.Now.ToString("dd.MM.yyyy");
        
        AddReview(new Review(comment, rating, clientName, date));
    }
    
    // Добавить отзыв в список и отобразить его
    public void AddReview(Review review)
    {
        // Добавляем в локальный список
        reviews.Add(review);
        
        // Сохраняем в GameDataManager
        ReviewData reviewData = new ReviewData(
            review.text,
            review.rating,
            review.clientName,
            review.date
        );
        GameDataManager.Instance.AddReview(reviewData);
        
        // Отображаем в UI
        DisplayReview(review);
    }
    
    // Отобразить отзыв в UI
    private void DisplayReview(Review review)
    {
        if (reviewPrefab == null || reviewsContainer == null)
        {
            Debug.LogError("ReviewPrefab или ReviewsContainer не назначены!");
            return;
        }
        
        GameObject reviewItem = Instantiate(reviewPrefab, reviewsContainer);
        ReviewItemUI reviewUI = reviewItem.GetComponent<ReviewItemUI>();
        
        if (reviewUI != null)
        {
            reviewUI.SetReview(review);
        }
        else
        {
            Debug.LogError("На префабе отзыва отсутствует компонент ReviewItemUI!");
        }
    }
    
    // Показать все отзывы
    public void DisplayAllReviews()
    {
        // Очищаем контейнер
        foreach (Transform child in reviewsContainer)
        {
            Destroy(child.gameObject);
        }
        
        // Отображаем все отзывы
        foreach (Review review in reviews)
        {
            DisplayReview(review);
        }
    }
    
    // Получить количество отзывов
    public int GetReviewCount()
    {
        return reviews.Count;
    }
    
    // Рассчитать средний рейтинг
    public float GetAverageRating()
    {
        // Теперь мы просто получаем рейтинг из GameDataManager
        return GameDataManager.Instance.GetRating();
    }
} 