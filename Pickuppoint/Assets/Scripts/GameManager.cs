using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Подписываемся на окончание дня
        TimeManager.Instance.OnDayEnded += EndDay;
    }

    private void EndDay()
    {
        Debug.Log("День завершен");
        // Здесь можно добавить дополнительную логику завершения дня
    }
}
