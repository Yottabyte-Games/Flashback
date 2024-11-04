using UnityEngine;

namespace Minigame.Fishing
{
    [RequireComponent(typeof(Reel))]
    public class Casting : MonoBehaviour
    {
        FishingRodInput input;

        public GameObject hook;
        public GameObject currentHook;
        Reel reel;
        Line fishingString;

        void Start()
        {
            fishingString = GetComponent<Line>();
            reel = GetComponent<Reel>();
            input = GetComponent<FishingRodInput>();
            input.cast += Cast;
            reel.FinishReel += ResetHook;
        }

        void Cast()
        {
            if (currentHook != null) return;

            currentHook = Instantiate(this.hook, transform.position, transform.rotation);

            if(fishingString.stringPoints.Count == 1)
                fishingString.stringPoints.Add(currentHook.transform);
            else
                fishingString.stringPoints[1] = currentHook.transform;

            Rigidbody hookRB = currentHook.GetComponent<Rigidbody>();
            Hook hook = currentHook.GetComponent<Hook>();

            hook.caughtFish += reel.StartReeling;
            hookRB.AddForce(transform.forward * 500, ForceMode.Force);
        }
        void ResetHook()
        {
            fishingString.stringPoints[1] = transform;
            Destroy(currentHook);
        }

    }
}
