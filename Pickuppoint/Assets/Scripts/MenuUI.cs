using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> _closedComponents;
    [SerializeField] private List<GameObject> _openComponents;
    [SerializeField] private List<Button> _buttons;
    public float minalpha = 170;
    public Color tmp;
    public void CloseAndOpen(int indexButton)
    {
        foreach (var component in _openComponents)
        {
            component.gameObject.SetActive(true);
        }
        foreach (var component in _closedComponents)
        {
            component.gameObject.SetActive(false);
        }
        foreach(var button in _buttons)
        {
            if (_buttons[indexButton] == button)
            {
                tmp.a = 1;
                button.image.color = tmp;
            }
            else
            {
                tmp.a = minalpha/256;
                button.image.color = tmp;
            }
        }
    }
}
