using UnityEngine;
using Utility.Physics;

namespace Minigame.Fishing
{
    [RequireComponent(typeof(Reel))]
    public class FishingRod : MonoBehaviour
    {
        [SerializeField] float timeToReelHook = 1;

        FishingRodInput input;

        public Hook hook;
        public Transform hookPoint;
        Reel reel;

        public bool HookThrowAnim { get; private set; }
        public bool HookIsCast { get; private set; }

        void Start()
        {
            reel = GetComponent<Reel>();
            input = GetComponent<FishingRodInput>();
            input.Cast += CastHook;
            hook.CaughtFish += reel.StartReeling;
        }

        void CastHook()
        {
            if (hook.fish != null) return;
            if (HookThrowAnim) return;

            if (HookIsCast)
                ReelHook();
            else
                hook.Cast();

            HookIsCast = !HookIsCast;
        }

        public async void ReelHook()
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
