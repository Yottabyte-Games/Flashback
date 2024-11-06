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

        public Rigidbody Rb { get; private set; }
        private void Start()
        {
            Rb = GetComponent<Rigidbody>();
        }
        public void CatchFish(Fish fishCaught)
        {
            if (fish != null) return;

            fishCaught.Catch(fishPos);
            //Rb.AddForce(Vector3.down * 500, ForceMode.Force);
            fish = fishCaught;
            CaughtFish?.Invoke(fishCaught);
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
            if(water != null)
                water.RemoveHook(this);
            Rb.isKinematic = true;
            UMethods.ResetTransform(transform, true);
        }
    }
}