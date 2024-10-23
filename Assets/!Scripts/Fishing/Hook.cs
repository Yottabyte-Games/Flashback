using System;
using UnityEngine;

namespace _Scripts
{
    public class Hook : MonoBehaviour
    {
        public Bait bait;
        public event Action<Fish> caughtFish;

        Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void CatchFish(Fish fishCaught)
        {
            rb.AddForce(Vector3.down * 500, ForceMode.Force);
            caughtFish?.Invoke(fishCaught);
        }
    }
}