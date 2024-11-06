using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using YottabyteGames.FinalCharacterController;

namespace Imp_Assets.GinjaGaming.FinalCharacterController.Scripts.Input
{
    [DefaultExecutionOrder(-2)]
    public class ThirdPersonInput : MonoBehaviour, PlayerControls.IThirdPersonMapActions
    {
        #region Class Variables
        public Vector2 ScrollInput { get; private set; }

        [SerializeField] CinemachineCamera _virtualCamera;
        [SerializeField] float _cameraZoomSpeed = 0.1f;
        [SerializeField] float _cameraMinZoom = 1f;
        [SerializeField] float _cameraMaxZoom = 5f;

        CinemachineThirdPersonFollow _thirdPersonFollow;
        #endregion

        #region Startup

        void Awake()
        {
            _thirdPersonFollow = _virtualCamera.GetComponent<CinemachineThirdPersonFollow>();
        }

        void OnEnable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot enable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Enable();
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.SetCallbacks(this);
        }

        void OnDisable()
        {
            if (PlayerInputManager.Instance?.PlayerControls == null)
            {
                Debug.LogError("Player controls is not initialized - cannot disable");
                return;
            }

            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.Disable();
            PlayerInputManager.Instance.PlayerControls.ThirdPersonMap.RemoveCallbacks(this);
        }
        #endregion

        #region Update

        void Update()
        {
            _thirdPersonFollow.CameraDistance = Mathf.Clamp(_thirdPersonFollow.CameraDistance + ScrollInput.y, _cameraMinZoom, _cameraMaxZoom);
        }

        void LateUpdate()
        {
            ScrollInput = Vector2.zero;
        }
        #endregion

        #region Input Callbacks
        public void OnScrollCamera(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            var scrollInput = context.ReadValue<Vector2>();
            ScrollInput = _cameraZoomSpeed * -1f * scrollInput.normalized;
        }
        #endregion
    }
}
