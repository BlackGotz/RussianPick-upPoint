using UnityEngine;

public class HurryClient : BaseClient
{
    protected override void Start()
    {
        // � ��������� ������� �������� ����, � �������� ����
        moveSpeed = 3f;      // �������� �������
        maxPatience = 5f;    // �������� ������������� �������
        base.Start();
    }
}
