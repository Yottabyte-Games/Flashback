using System;
using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics
{
    public class CpMain : MonoBehaviour
    {
        public PlayerInputs input;
        public VehicleSpeed speedData;
        public CpWheelData wheelData;

        [HideInInspector] public Rigidbody rb;
        public Vector3 averageColliderSurfaceNormal;

        bool _prevGroundedState;
        public static event Action<CpMain> OnLeavingGround = cpMain => { };
        public static event Action<CpMain> OnLanding = cpMain => { };

        void Awake()
        {
            rb = GetComponentInChildren<Rigidbody>();
        }

        void Update()
        {
            if (_prevGroundedState == false && wheelData.grounded)
            {
                OnLanding(this);
            }
            else if (_prevGroundedState == true && !wheelData.grounded)
            {
                OnLeavingGround(this);
            }

            _prevGroundedState = wheelData.grounded;
        }
    }
}
