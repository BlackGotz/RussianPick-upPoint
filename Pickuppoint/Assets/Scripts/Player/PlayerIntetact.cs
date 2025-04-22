using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerIntetact : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask obstacleMask; // ��������� ����� ��� �����������
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
        //������� ��� �� ������ ������ ������
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; //�������� ����������, ���� ����� ���

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            // ���������, ���� �� ����������� ����� ������� � ������������� ��������
            if (!HasObstacleBetween(ray.origin, hitInfo.point))
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    currentInteractable = interactable;
                    playerUI.UpdateText(interactable.promptMessage);

                    // ��������� ��������������
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

        // ��������� ������ �������� �� ��������������
        if (inputManager.onFoot.Throw.triggered && currentInteractable != null)
        {
            Debug.Log("������� ������ " + gameObject.name);
            Vector3 throwDirection = ray.direction;  // ���������� ����������� ����
            currentInteractable.BaseThrow(throwDirection);
        }

        if (inputManager.onChange.OpenMenu.triggered)
        {
            Debug.Log("������� ����");
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

    // ����� ��� �������� ������� ����������� ����� ����� �������
    private bool HasObstacleBetween(Vector3 start, Vector3 end)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(start, direction.normalized, out hit, distance, obstacleMask))
        {
            // ���� ��� ����� � ���-�� �� obstacleMask �� ���������� �������� �����
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