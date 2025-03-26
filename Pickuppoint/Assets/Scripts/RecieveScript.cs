using UnityEngine;

public class RecieveScript : MonoBehaviour
{
    private BaseClient currentClient;

    private void Start()
    {
        // Ищем компонент-наследник BaseClient один раз при старте
        currentClient = GetComponentInParent<BaseClient>();

        if (currentClient == null)
        {
            Debug.LogError($"Ошибка: BaseClient не найден в родительских объектах {gameObject.name}!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Box box = other.GetComponent<Box>(); // Проверяем, является ли вошедший объект коробкой
        Interactable item = other.GetComponent<Interactable>();
        if (item.GetIsThrown())
            {
                Debug.Log("Предмет был брошен!");
            }
        if (box != null)
        {
            if (currentClient != null)
            {
                currentClient.ReceiveBox(box);
                //Debug.Log($"Box {box.name} передан клиенту {currentClient.name}");
            }
            else
            {
                Debug.LogError("Ошибка: текущий клиент (BaseClient) не найден!");
            }
        }
    }
}
