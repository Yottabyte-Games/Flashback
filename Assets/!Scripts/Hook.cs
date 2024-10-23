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

    public enum BaitType
    {
        None = 0,
        Bread = 1,
        Worm = 2,
        Leech = 3,
        ChickenLiver = 4,
        Shrimp = 5,
    }
    [Serializable]
    public class Bait
    {
        public BaitType type;
    }
}