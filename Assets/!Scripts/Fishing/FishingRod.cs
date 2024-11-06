using UnityEngine;

namespace _Scripts.Fishing
{
    [RequireComponent(typeof(Reel))]
    public class FishingRod : MonoBehaviour
    {
        FishingRodInput _input;

        public Hook hook;
        public Transform hookPoint;
        Reel _reel;

        [SerializeField] SpringJoint line;
        void Start()
        {
            _reel = GetComponent<Reel>();
            _input = GetComponent<FishingRodInput>();
            _input.Cast += CastHook;
            hook.CaughtFish += _reel.StartReeling;
            _reel.FinishReel += ResetHook;
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
