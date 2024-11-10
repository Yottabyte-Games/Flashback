using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Working
{
    public class WorkInput : MonoBehaviour, WorkInputs.IWorkActions
    {
        WorkInputs _input;
        public event Action interact;
        WorkInteractable _toInteractWith;

<<<<<<< HEAD:Assets/Scenes/Working/WorkInput.cs
    void OnEnable()
    {
        input = new WorkInputs();
        input.Work.Enable();
        input.Work.SetCallbacks(this);
    }

    void Start()
    {
        interact += Interact;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WorkInteractable interactable))
=======
        private void OnEnable()
>>>>>>> Build:Assets/!Scripts/Working/WorkInput.cs
        {
            _input = new WorkInputs();
            _input.Work.Enable();
            _input.Work.SetCallbacks(this);
        }
        private void Start()
        {
            interact += Interact;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out WorkInteractable interactable))
            {
                if (interactable.enabled == false) return;
                _toInteractWith = interactable;
            }
        }

        void Interact()
        {
            if (_toInteractWith == null) return;
            _toInteractWith.interact.Invoke(transform);
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            interact?.Invoke();
        }
    }
}
