using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.Fishing
{
    public class FishingRodInput : MonoBehaviour, FishingInputs.IFishingActions
    {
        FishingInputs _input;

        public event Action Cast;
        public event Action<Vector2> Reel;

        private void OnEnable()
        {
            _input = new FishingInputs();
            _input.Fishing.Enable();
            _input.Fishing.SetCallbacks(this);
        }
        private void OnDisable()
        {
            _input.Disable();
            _input.Fishing.RemoveCallbacks(this);
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
