using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    [SerializeField] private Transform _door;
    public bool open = false;
    public float speed = 90f; // Градусы в секунду
    private float targetAngle;
    private float closedAngle;
    private float openAngle;
    public float side = 1f;
    private void Start()
    {
        // Устанавливаем начальный угол двери
        closedAngle = _door.localEulerAngles.y;
        openAngle = closedAngle + side*90f; // Поворот на 180 градусов
        targetAngle = closedAngle;
    }

    protected override void Interact()
    {
        open = !open;
        targetAngle = open ? openAngle : closedAngle;
        promptMessage = open ? "Press 'E' to close" : "Press 'E' to open";
    }

    void Update()
    {
        float currentAngle = _door.localEulerAngles.y;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, speed * Time.deltaTime);
        _door.localEulerAngles = new Vector3(0, newAngle, 0);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Client"))
        {
            //Debug.Log("enter");
            open = true;
            targetAngle = openAngle;
            promptMessage = "Press 'E' to close";
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Client"))
        {
            //Debug.Log("exit");
            open = false;
            targetAngle = closedAngle;
            promptMessage = "Press 'E' to open";
        }
    }
}
