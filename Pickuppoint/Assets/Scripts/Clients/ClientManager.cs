using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    // Вызывается когда клиент уходит (доволен или нет)
    public void ClientLeft(BaseClient client, bool satisfied)
    {
        // Рассчитываем процент терпения клиента
        float patiencePercent = client.currentPatience / client.maxPatience;
        
        // Если клиент получил свою посылку, он доволен
        if (satisfied)
        {
            // Даже если терпение на нуле, но клиент получил посылку, 
            // считаем что его терпение было как минимум средним
            patiencePercent = Mathf.Max(patiencePercent, 0.4f);
            
            // Сообщаем GameManager о довольном клиенте
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ClientServed(true);
            }
        }
        else
        {
            // Если клиент не получил посылку, его терпение считаем нулевым
            patiencePercent = 0f;
            
            // Сообщаем GameManager о недовольном клиенте
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ClientServed(false);
            }
        }
        
        // Создаем отзыв на основе терпения клиента
        if (ReviewSystem.Instance != null)
        {
            ReviewSystem.Instance.CreateReviewFromPatience(patiencePercent);
        }
    }
} 