using UnityEngine;

namespace _Scripts.Fishing
{
    [RequireComponent(typeof(Reel))]
    public class FishingRod : MonoBehaviour
    {
        FishingRodInput input;

        public Hook hook;
        public Transform hookPoint;
        Reel reel;

        [SerializeField] SpringJoint line;
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
            line.spring = 0;
            hook.Cast();
        }
        void ResetHook()
        {
            hook.transform.parent = hookPoint;
            hook.Ready();
        }
        public void AddPullStrength(float magnitude)
        {
            line.spring += magnitude;
        }
    }
}
