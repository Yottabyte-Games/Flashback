using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Minigame.Fishing
{
    public class FishingRodInput : MonoBehaviour, FishingInputs.IFishingActions
    {
        FishingInputs input;

        public event Action cast;
        public event Action<Vector2> reel;

        private void OnEnable()
        {
            input = new FishingInputs();
            input.Fishing.Enable();
            input.Fishing.SetCallbacks(this);
        }
        private void OnDisable()
        {
            input.Disable();
            input.Fishing.RemoveCallbacks(this);
        }
        public void OnCast(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            cast?.Invoke();
        }

        public void OnReel(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            reel?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
