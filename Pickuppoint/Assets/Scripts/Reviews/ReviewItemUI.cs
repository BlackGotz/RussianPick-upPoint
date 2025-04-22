using UnityEngine;
using TMPro;

public class ReviewItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI reviewText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI clientNameText;
    [SerializeField] private TextMeshProUGUI dateText;
    
    // Настройка отображения отзыва
    public void SetReview(Review review)
    {
        // Установка текста отзыва
        if (reviewText != null)
            reviewText.text = review.text;
            
        // Установка рейтинга текстом
        if (ratingText != null)
            ratingText.text = review.rating.ToString() + "/5";
            
        // Установка имени клиента
        if (clientNameText != null)
            clientNameText.text = review.clientName;
            
        // Установка даты
        if (dateText != null)
            dateText.text = review.date;
    }
} 