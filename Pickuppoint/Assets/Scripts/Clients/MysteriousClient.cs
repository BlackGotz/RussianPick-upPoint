using UnityEngine;

public class MysteriousClient : BaseClient
{
    private int chosenIndex = -1;
    private bool hintShown = false;

    // Переопределяем метод SetupClient для определения случайной позиции подсказки
    public override void SetupClient(int boxNumber, Transform targetPoint)
    {
        base.SetupClient(boxNumber, targetPoint);
        string orderStr = requiredBoxNumber.ToString();
        chosenIndex = Random.Range(0, orderStr.Length);
    }

    // Переопределяем отображаемый текст
    // Если подсказка уже показана, возвращаем замаскированную строку (например, "***3*"),
    // иначе — только выбранную цифру
    protected override string GetDisplayText()
    {
        string orderStr = requiredBoxNumber.ToString();
        if (hintShown)
        {
            char[] masked = new char[orderStr.Length];
            for (int i = 0; i < orderStr.Length; i++)
            {
                masked[i] = (i == chosenIndex) ? orderStr[i] : '*';
            }
            return new string(masked);
        }
        else
        {
            return orderStr[chosenIndex].ToString();
        }
    }

    // Переопределяем Update, добавляя проверку для показа подсказки
    protected override void Update()
    {
        // Вызываем базовую логику перемещения, ожидания и ухода
        base.Update();

        // Если клиент в режиме ожидания, и его терпение упало ниже половины,
        // а подсказка ещё не показана – обновляем дисплей
        if (state == ClientState.Waiting && !hintShown && currentPatience <= maxPatience / 2f)
        {
            hintShown = true;
            UIManager.Instance.ShowClientPhone(GetDisplayText());
        }
    }
}
