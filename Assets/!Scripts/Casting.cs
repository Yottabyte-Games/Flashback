using UnityEngine;

namespace Minigame.Fishing
{
    [RequireComponent(typeof(Reel))]
    public class Casting : MonoBehaviour
    {
        public GameObject hook;
        String fishingString;

        void Start()
        {
            fishingString = GetComponent<String>();
            Cast();
        }

        void Cast()
        {
            GameObject currentHook = Instantiate(hook, transform.position, transform.rotation);
            fishingString.stringPoints.Add(currentHook.transform);
            Rigidbody hookRB = currentHook.GetComponent<Rigidbody>();
            hookRB.AddForce(transform.forward * 500, ForceMode.Force);
        }

    }
}
