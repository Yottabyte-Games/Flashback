using System.Threading.Tasks;
using UnityEngine;
using Utility.Physics;

namespace Minigame.Fishing
{
    [RequireComponent(typeof(Reel))]
    public class FishingRod : MonoBehaviour
    {
        FishingRodInput input;

        public Hook hook;
        public Transform hookPoint;
        Reel reel;

        void Start()
        {
            reel = GetComponent<Reel>();
            input = GetComponent<FishingRodInput>();
            input.Cast += CastHook;
            hook.CaughtFish += reel.StartReeling;
            reel.FinishReel += ResetHook;
        }

        void CastHook()
        {
            hook.Cast();
        }

        public async Task ReelHook()
        {
            await UPhysics.ThrowToAsync(hook.rb, hookPoint.position);
        }

        void ResetHook()
        {
            hook.transform.parent = hookPoint;
            hook.Ready();
        }
    }
}
