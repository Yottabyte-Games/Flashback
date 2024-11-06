using System;
using Unity.Collections;
using UnityEngine;

namespace Minigame.Fishing
{
    public class Hook : MonoBehaviour
    {
        [SerializeField] float hookWeight = 10f;
        [SerializeField] Transform fishPos;

        [ReadOnly] public Fish fish;
        public Bait bait;
        public event Action<Fish> CaughtFish;

        public Rigidbody rb { get; private set; }
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void CatchFish(Fish fishCaught)
        {
            fishCaught.transform.parent = fishPos;
            fishCaught.transform.localPosition = Vector3.zero;
            fishCaught.transform.localEulerAngles = Vector3.zero;
            rb.AddForce(Vector3.down * 5000, ForceMode.Force);
            fish = fishCaught;
            CaughtFish?.Invoke(fishCaught);
        }
        public void Cast()
        {
            transform.parent = null;
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 5000, ForceMode.Force);
        }
        public void Ready()
        {
            rb.mass = hookWeight;
            rb.isKinematic = true;
            transform.localPosition = Vector3.zero;
        }
    }
}