using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputActions;

namespace GitAmendKartSample.Scripts {
    [CreateAssetMenu(fileName = "InputReader", menuName = "Kart/Input Reader")]
    public class InputReader : ScriptableObject, IPlayerActions, IDrive {
        public Vector2 Move => _inputActions.Player.Move.ReadValue<Vector2>();
        public bool IsBraking => _inputActions.Player.Brake.ReadValue<float>() > 0;

        PlayerInputActions _inputActions;
        
        void OnEnable()
        {
            if (_inputActions is not null) return;
            _inputActions = new PlayerInputActions();
            _inputActions.Player.SetCallbacks(this);
        }
        
        public void Enable() {
            _inputActions.Enable();
        }
        
        public void OnMove(InputAction.CallbackContext context) {
            // noop
        }

        public void OnLook(InputAction.CallbackContext context) {
            // noop
        }

        public void OnFire(InputAction.CallbackContext context) {
            // noop
        }

        public void OnBrake(InputAction.CallbackContext context) {
            // noop
        }
    }
}
