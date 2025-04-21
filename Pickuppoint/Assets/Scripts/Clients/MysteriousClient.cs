using UnityEngine;
using System.Collections.Generic;

public class MysteriousClient : BaseClient
{
    private List<int> revealedIndices = new List<int>();
    private bool hintShown = false;

    public override void SetupClient(int boxNumber, Transform targetPoint)
    {
        base.SetupClient(boxNumber, targetPoint);

        // �������� 2 ��������� ���������� ������� ��� ������
        revealedIndices.Clear();
        while (revealedIndices.Count < 2)
        {
            int index = Random.Range(0, requiredBoxNumber.ToString().Length);
            if (!revealedIndices.Contains(index))
                revealedIndices.Add(index);
        }
    }

    protected override string GetDisplayText()
    {
        string orderStr = requiredBoxNumber.ToString();

        if (hintShown)
        {
            char[] masked = new char[orderStr.Length];
            for (int i = 0; i < orderStr.Length; i++)
            {
                masked[i] = revealedIndices.Contains(i) ? orderStr[i] : '*';
            }
            return new string(masked);
        }
        else
        {
            // ���������� ���� �� ���� ���� ��� ������ ���������
            return orderStr[revealedIndices[0]].ToString();
        }
    }

    protected override void Update()
    {
        base.Update();

        if (state == ClientState.Waiting && !hintShown && currentPatience <= maxPatience / 1.5f)
        {
            hintShown = true;
            UIManager.Instance.ShowClientPhone(GetDisplayText());
        }
    }
}
