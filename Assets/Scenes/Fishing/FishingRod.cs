<<<<<<< HEAD:Assets/Scenes/Fishing/FishingRod.cs
using _Scripts.Fishing;
=======
using Imp_Assets.GinjaGaming.FinalCharacterController.Scripts;
using System.Threading.Tasks;
>>>>>>> Build:Assets/!Scripts/Fishing/FishingRod.cs
using UnityEngine;
using Utility.Physics;

namespace _Scripts.Fishing
{
    [RequireComponent(typeof(Reel))]
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] float timeToReelHook = 1;

        FishingRodInput input;

        public Hook hook;
        public Transform hookPoint;
        Reel reel;
        PlayerController player;


        public bool HookThrowAnim { get; private set; }
        public bool HookIsCast { get; private set; }

        void Start()
        {
            player = FindFirstObjectByType<PlayerController>();
            reel = GetComponent<Reel>();
            input = GetComponent<FishingRodInput>();
            input.Cast += CastHook;
            hook.connectedRod = this;


            ToggleReeling(false);
        }

        void CastHook()
        {
            if (hook.fish != null) return;
            if (HookThrowAnim) return;

            if (HookIsCast)
                _ = ReelHook();
            else
                hook.Cast();

            HookIsCast = !HookIsCast;
        }
        public void ToggleReeling(bool toggle)
        {
            player.ToggleCameraMovement(!toggle);
            reel.enabled = toggle;
        }
        public async Task ReelHook()
        {
            if (!hook.Rb.useGravity)
                hook.Rb.useGravity = true;

            HookThrowAnim = true;
            await UPhysics.ThrowToAsync(hook.Rb, hookPoint.position, (int)(timeToReelHook * 1000));
            HookThrowAnim = false;

            ResetHook();
        }

        void ResetHook()
        {
            hook.transform.parent = hookPoint;
            hook.Ready();
            HookIsCast = false;
        }
    }
}
