using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerUIController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _closedComponents;
    [SerializeField] private List<GameObject> _openComponents;
    
    public void CloseAndOpen()
    {
        foreach (var component in _openComponents)
        {
            component.gameObject.SetActive(true);
        }
        foreach (var component in _closedComponents)
        {
            component.gameObject.SetActive(false);
        }
    }
}
