using Minigame.Fishing;
using UnityEngine;

namespace _Scripts
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
            var hookRB = currentHook.GetComponent<Rigidbody>();
            var hook = currentHook.GetComponent<Hook>();

            hook.caughtFish += reel.StartReeling;
            hookRB.AddForce(transform.forward * 500, ForceMode.Force);
        }

    }
}
