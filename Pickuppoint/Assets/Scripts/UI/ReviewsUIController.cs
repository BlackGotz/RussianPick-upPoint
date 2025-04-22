using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReviewsUIController : MonoBehaviour
{
    [SerializeField] private GameObject reviewsPanel;
    [SerializeField] private TextMeshProUGUI averageRatingText;
    [SerializeField] private TextMeshProUGUI totalReviewsText;
    
    [SerializeField] private List<GameObject> mainMenuComponents; // Компоненты, которые нужно включить при закрытии панели отзывов
    
    private void Start()
    {
        // Скрываем панель отзывов по умолчанию
        if (reviewsPanel != null)
            reviewsPanel.SetActive(false);
    }
    
    // Метод для открытия панели отзывов
    public void OpenReviewsPanel()
    {
        // Скрываем элементы основного меню
        foreach (var component in mainMenuComponents)
        {
            if (component != null)
                component.gameObject.SetActive(false);
        }
        
        // Активируем панель отзывов
        if (reviewsPanel != null)
        {
            reviewsPanel.SetActive(true);
            
            // Отображаем все отзывы через ReviewSystem
            if (ReviewSystem.Instance != null)
            {
                ReviewSystem.Instance.DisplayAllReviews();
                UpdateReviewsStatistics();
            }
            else
            {
                Debug.LogError("ReviewSystem.Instance не найден!");
            }
        }
    }
    
    // Обновляет статистику отзывов (средний рейтинг, количество отзывов)
    private void UpdateReviewsStatistics()
    {
        if (ReviewSystem.Instance == null)
            return;
            
        // Обновляем средний рейтинг
        if (averageRatingText != null)
        {
            float averageRating = ReviewSystem.Instance.GetAverageRating();
            averageRatingText.text = string.Format("Средний рейтинг: {0:0.0}/5", averageRating);
        }
        
        // Обновляем количество отзывов
        if (totalReviewsText != null)
        {
            int reviewCount = ReviewSystem.Instance.GetReviewCount();
            totalReviewsText.text = string.Format("Всего отзывов: {0}", reviewCount);
        }
    }
    
    // Метод для закрытия панели отзывов
    public void CloseReviewsPanel()
    {
        if (reviewsPanel != null)
            reviewsPanel.SetActive(false);
            
        // Возвращаемся к главному меню компьютера
        foreach (var component in mainMenuComponents)
        {
            if (component != null)
                component.gameObject.SetActive(true);
        }
    }
} 