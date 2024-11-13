using System;
using Unity.Collections;
using UnityEngine;
using Utility.Methods;

namespace _Scripts.Fishing
{
    public class Hook : MonoBehaviour
    {
        public FishingRod connectedRod;

        [SerializeField] Transform fishPos;

        [ReadOnly] public Fish fish;
        public Bait bait;
        public event Action<Fish> CaughtFish;

        [HideInInspector] public FishWater water;

        public Rigidbody Rb { get; private set; }

        void Start()
        {
            Rb = GetComponent<Rigidbody>();
        }
        public void CatchFish(Fish fishCaught)
        {
            if (fish != null) return;

            fishCaught.Catch(fishPos);
            fish = fishCaught;
            CaughtFish?.Invoke(fishCaught);

            connectedRod.ToggleReeling(true);
        }
        public void Cast()
        {
            print("Cast");
            transform.parent = null;
            Rb.isKinematic = false;
            Rb.AddForce(transform.forward * 500, ForceMode.Force);
        }
        public void Ready()
        {
            if (water != null)
                water.RemoveHook(this);

            Rb.isKinematic = true;
            UMethods.ResetTransform(transform, true);
        }
    }
}