using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerIntetact : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    private PlayerUI playerUI;
    private InputManager inputManager;
    private Interactable currentInteractable;
    [SerializeField] private GameObject menu;
    public bool menuisopen = false;
    public VolumeControl volumeControl;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();

    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        //Пускаем луч от центра камеры игрока
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //Собираем информацию, куда попал луч
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                playerUI.UpdateText(interactable.promptMessage);

                // Проверяем взаимодействие
                if (inputManager.onFoot.Interact.triggered)
                {
                    //Debug.Log("Условие взятия " + gameObject.name);
                    interactable.BaseInteract();
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
            if(menuisopen)
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
