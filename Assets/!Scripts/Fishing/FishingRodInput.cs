using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Fishing
{
    public class FishingRodInput : MonoBehaviour, FishingInputs.IFishingActions
    {
        FishingInputs input;

        public event Action Cast;
        public event Action<Vector2> Reel;

        void OnEnable()
        {
            input = new FishingInputs();
            input.Fishing.Enable();
            input.Fishing.SetCallbacks(this);
        }

        void OnDisable()
        {
            input.Disable();
            input.Fishing.RemoveCallbacks(this);
        }
        public void OnCast(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            Cast?.Invoke();
        }

        public void OnReel(InputAction.CallbackContext context)
        {
            Reel?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
