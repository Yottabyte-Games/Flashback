using GinjaGaming.FinalCharacterController;
using UnityEngine;

namespace Imp_Assets.GinjaGaming.FinalCharacterController.Scripts.Input
{
    [DefaultExecutionOrder(-3)]
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance;
        public PlayerControls PlayerControls {  get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();
        }

        void OnDisable()
        {
            PlayerControls.Disable();
        }
    }
}
