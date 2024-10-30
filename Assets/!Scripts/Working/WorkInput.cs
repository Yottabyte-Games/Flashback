using _Scripts.Snowman_Scripts.Interaction;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class WorkInput : MonoBehaviour, WorkInputs.IWorkActions
{
    WorkInputs input;
    public event Action interact;
    WorkInteractable toInteractWith;

    private void OnEnable()
    {
        input = new WorkInputs();
        input.Work.Enable();
        input.Work.SetCallbacks(this);
    }
    private void Start()
    {
        interact += Interact;    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out WorkInteractable interactable))
        {
            if (interactable.enabled == false) return;
            toInteractWith = interactable;
        }
    }

    void Interact()
    {
        if (toInteractWith == null) return;
        toInteractWith.interact.Invoke(transform);
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        interact?.Invoke();
    }
}
