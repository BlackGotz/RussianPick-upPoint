using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;

    public void BaseInteract()
    {
        Interact();
    }
    public void BaseThrow(Vector3 throwDirection)
    {
        Throw(throwDirection);
    }


    protected virtual void Interact()
    {

    }

    protected virtual void Throw(Vector3 direction)
    {

    }


}
