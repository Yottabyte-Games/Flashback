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
        String fishingString;

        void Start()
        {
            fishingString = GetComponent<String>();
            reel = GetComponent<Reel>();
            input = GetComponent<FishingRodInput>();
            input.cast += Cast;
        }

        void Cast()
        {
            if (currentHook != null) return;

            currentHook = Instantiate(this.hook, transform.position, transform.rotation);
            fishingString.stringPoints.Add(currentHook.transform);
            Rigidbody hookRB = currentHook.GetComponent<Rigidbody>();
            Hook hook = currentHook.GetComponent<Hook>();

            hook.caughtFish += reel.StartReeling;
            hookRB.AddForce(transform.forward * 500, ForceMode.Force);
        }

    }
}
