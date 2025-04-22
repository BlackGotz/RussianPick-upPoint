using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerIntetact : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask obstacleMask; // Добавляем маску для препятствий
    private PlayerUI playerUI;
    private InputManager inputManager;
    private Interactable currentInteractable;
    [SerializeField] private GameObject menu;
    public bool menuisopen = false;
    public VolumeControl volumeControl;

    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        //Пускаем луч от центра камеры игрока
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //Собираем информацию, куда попал луч

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            // Проверяем, есть ли препятствие между игроком и интерактивным объектом
            if (!HasObstacleBetween(ray.origin, hitInfo.point))
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    playerUI.UpdateText(interactable.promptMessage);

                    // Проверяем взаимодействие
                    if (inputManager.onFoot.Interact.triggered)
                    {
                        interactable.BaseInteract();
                    }
                }
                else
                {
                    currentInteractable = null;
                }
            }
            else
            {
                currentInteractable = null;
            }
        }

        // Проверяем бросок отдельно от взаимодействия
        if (inputManager.onFoot.Throw.triggered && currentInteractable != null)
        {
            Debug.Log("Условие броска " + gameObject.name);
            Vector3 throwDirection = ray.direction;  // Используем направление луча
            currentInteractable.BaseThrow(throwDirection);
        }

        if (inputManager.onChange.OpenMenu.triggered)
        {
            Debug.Log("Открыть меню");
            menu.SetActive(!menuisopen);
            if (menuisopen)
            {
                inputManager.onFoot.Enable();
            }
            else
            {
                inputManager.onFoot.Disable();
            }
            menuisopen = !menuisopen;

            volumeControl.RevertSettings();
        }
    }

    // Метод для проверки наличия препятствий между двумя точками
    private bool HasObstacleBetween(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(start, direction.normalized, out hit, distance, obstacleMask))
        {
            // Если луч попал в что-то из obstacleMask до достижения конечной точки
            return true;
        }
        return false;
    }

    public void CloseMenu()
    {
        menu.SetActive(!menuisopen);
        if (menuisopen)
        {
            inputManager.onFoot.Enable();
        }
        else
        {
            inputManager.onFoot.Disable();
        }
        menuisopen = !menuisopen;
    }
}