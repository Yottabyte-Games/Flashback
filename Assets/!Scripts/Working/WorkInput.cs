using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Working
{
    public class WorkInput : MonoBehaviour, WorkInputs.IWorkActions
    {
        WorkInputs _input;
        public event Action interact;

        void OnEnable()
        {
            _input = new WorkInputs();
            _input.Work.Enable();
            _input.Work.SetCallbacks(this);
        }

        void Start()
        {
            interact += Interact;
        }

        void Interact()
        {
            WorkInteractable _toInteractWith = null;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 5))
            {
                hit.collider.TryGetComponent(out _toInteractWith);
            }

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
