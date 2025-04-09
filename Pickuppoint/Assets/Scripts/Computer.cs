using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Computer : Interactable
{
    bool show = false;
    public GameObject _playerCamera;
    public GameObject _computerCamera;
    public GameObject _screen;

    private PlayerInput playerInput;


    public PlayerInput.UIActions UI;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        UI = playerInput.UI;
    }
    protected override void Interact()
    {
        if (!show)
        {
            show = true;
            UI.Enable();
            _playerCamera.SetActive(false);
            _computerCamera.SetActive(true);
            _screen.SetActive(true);
        }
        else
        {
            _playerCamera.SetActive(true);
            _computerCamera.SetActive(false);
            _screen.SetActive(false);
            UI.Disable();
            show = false;
        }
    }

    public void ExitComputer()
    {
        show = false;
        UI.Disable();
        _playerCamera.SetActive(true);
        _computerCamera.SetActive(false);
    }
    
}
