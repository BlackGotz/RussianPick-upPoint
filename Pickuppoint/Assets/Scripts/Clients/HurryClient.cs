using UnityEngine;

public class HurryClient : BaseClient
{
    protected override void Start()
    {
        // � ��������� ������� �������� ����, � �������� ����
        moveSpeed = 3f;      // �������� �������
        maxPatience = 20f;    // �������� ������������� �������
        base.Start();
    }
}
