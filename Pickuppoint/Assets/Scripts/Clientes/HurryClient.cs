using UnityEngine;

public class HurryClient : BaseClient
{
    protected override void Start()
    {
        // У спешащего клиента скорость выше, а терпение ниже
        moveSpeed = 3f;      // Движется быстрее
        maxPatience = 5f;    // Терпение заканчивается быстрее
        base.Start();
    }
}
