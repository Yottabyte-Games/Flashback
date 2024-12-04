using _Scripts.Generic;
using GinjaGaming.FinalCharacterController;
using UnityEngine;

namespace Imp_Assets.GinjaGaming.FinalCharacterController.Scripts.Input
{
    [DefaultExecutionOrder(-3)]
    public class PlayerInputManager : Singleton<PlayerInputManager>
    {
        public PlayerControls PlayerControls {  get; private set; }

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
