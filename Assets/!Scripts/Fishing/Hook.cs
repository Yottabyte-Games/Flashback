using System;
using Unity.Collections;
using UnityEngine;
using Utility.Methods;

namespace Minigame.Fishing
{
    public class Hook : MonoBehaviour
    {
        [SerializeField] Transform fishPos;

        [ReadOnly] public Fish fish;
        public Bait bait;
        public event Action<Fish> CaughtFish;

        [HideInInspector] public FishWater water;

        public Rigidbody rb { get; private set; }
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void CatchFish(Fish fishCaught)
        {
            fishCaught.Catch(fishPos);
            rb.AddForce(Vector3.down * 500, ForceMode.Force);
            fish = fishCaught;
            CaughtFish?.Invoke(fishCaught);
        }
        public void Cast()
        {
            transform.parent = null;
            rb.isKinematic = false;
            rb.AddForce(transform.forward * 500, ForceMode.Force);
        }
        public void Ready()
        {
            water.RemoveHook(this);
            rb.isKinematic = true;
            UMethods.ResetTransform(transform, true);
        }
    }
}