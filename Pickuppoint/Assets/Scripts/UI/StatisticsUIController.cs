using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatisticsUIController : MonoBehaviour
{
    [Header("Элементы пользовательского интерфейса")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI reviewsCountText;
    
    [Header("Настройки отображения")]
    [SerializeField] private string balanceFormat = "Баланс: {0} ₽";
    [SerializeField] private string dayFormat = "День: {0}";
    [SerializeField] private string ratingFormat = "Рейтинг: {0:0.0}/5";
    [SerializeField] private string reviewsCountFormat = "Отзывы: {0}";
    
    private void Start()
    {
        // Обновляем UI при старте
        UpdateUI();
    }
    
    // Обновляет все элементы пользовательского интерфейса
    public void UpdateUI()
    {
        // Загружаем данные из файла через GameDataManager
        UpdateBalance();
        UpdateDay();
        UpdateRating();
        UpdateReviewsCount();
    }
    
    // Обновляет отображение баланса
    public void UpdateBalance()
    {
        if (balanceText != null)
        {
            int balance = GameDataManager.Instance.GetBalance();
            balanceText.text = string.Format(balanceFormat, balance);
        }
    }
    
    // Обновляет отображение текущего дня
    public void UpdateDay()
    {
        if (dayText != null)
        {
            int day = GameDataManager.Instance.GetCurrentDay();
            dayText.text = string.Format(dayFormat, day);
        }
    }
    
    // Обновляет отображение рейтинга
    public void UpdateRating()
    {
        if (ratingText != null)
        {
            float rating = GameDataManager.Instance.GetRating();
            ratingText.text = string.Format(ratingFormat, rating);
        }
    }
    
    // Обновляет отображение количества отзывов
    public void UpdateReviewsCount()
    {
        if (reviewsCountText != null)
        {
            int reviewsCount = GameDataManager.Instance.GetReviews().Count;
            reviewsCountText.text = string.Format(reviewsCountFormat, reviewsCount);
        }
    }
    
    // Метод для прямого установления значения рейтинга
    public void SetRating(float rating)
    {
        // Сохраняем рейтинг через GameDataManager
        GameDataManager.Instance.SetRating(rating);
        
        // Обновляем отображение
        UpdateRating();
    }
} 