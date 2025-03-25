using UnityEngine;

public class MysteriousClient : BaseClient
{
    private int chosenIndex = -1;
    private bool hintShown = false;

    // �������������� ����� SetupClient ��� ����������� ��������� ������� ���������
    public override void SetupClient(int boxNumber, Transform targetPoint)
    {
        base.SetupClient(boxNumber, targetPoint);
        string orderStr = requiredBoxNumber.ToString();
        chosenIndex = Random.Range(0, orderStr.Length);
    }

    // �������������� ������������ �����
    // ���� ��������� ��� ��������, ���������� ��������������� ������ (��������, "***3*"),
    // ����� � ������ ��������� �����
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

    // �������������� Update, �������� �������� ��� ������ ���������
    protected override void Update()
    {
        // �������� ������� ������ �����������, �������� � �����
        base.Update();

        // ���� ������ � ������ ��������, � ��� �������� ����� ���� ��������,
        // � ��������� ��� �� �������� � ��������� �������
        if (state == ClientState.Waiting && !hintShown && currentPatience <= maxPatience / 2f)
        {
            hintShown = true;
            UIManager.Instance.ShowClientPhone(GetDisplayText());
        }
    }
}
