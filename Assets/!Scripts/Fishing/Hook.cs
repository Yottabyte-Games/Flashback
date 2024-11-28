using System;
using Unity.Collections;
using UnityEngine;

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

        public SpringJoint Spring { get; private set; }
        float springValue;
        float mass;
        float drag;
        public Rigidbody Rb { get; private set; }

        void Start()
        {
            Spring = GetComponent<SpringJoint>();
            springValue = Spring.spring;
            Rb = GetComponent<Rigidbody>();
            mass = Rb.mass;
            drag = Rb.linearDamping;
            transform.parent = null;
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
            Spring.spring = 0;
            Rb.mass = 1;
            Rb.linearDamping = 0;
            Rb.AddForce(connectedRod.transform.forward * 500, ForceMode.Force);
        }
        public void Ready()
        {
            if (water != null)
                water.RemoveHook(this);

            Rb.linearVelocity = Vector3.zero;
            Rb.mass = mass;
            Rb.linearDamping = drag;
            Spring.spring = springValue;
        }
    }
}