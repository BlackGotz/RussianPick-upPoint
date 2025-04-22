using UnityEngine;

public class HurryClient : BaseClient
{
    protected override void Start()
    {
        // У спешащего клиента скорость выше, а терпение ниже
        moveSpeed = 3f;      // Движется быстрее
        maxPatience = 20f;    // Терпение заканчивается быстрее
        base.Start();
    }
}
