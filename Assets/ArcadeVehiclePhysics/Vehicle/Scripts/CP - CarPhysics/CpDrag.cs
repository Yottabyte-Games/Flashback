using System;
using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics
{
    [Serializable]
    public class CpDrag : MonoBehaviour
    {
        [Header("Drag")]
        public float linearDrag;
        public float freeWheelDrag;
        public float brakingDrag;
        public float angularDrag;

        public bool linearDragCheck;
        public bool brakingDragCheck;
        public bool freeWheelDragCheck;

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = transform.parent.GetComponent<CpMain>();

            _cpMain.rb.angularDamping = angularDrag;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateDrag(
                _cpMain.rb,
                _cpMain.wheelData.grounded,
                _cpMain.input,
                _cpMain.speedData
            );

        }

        void UpdateDrag(Rigidbody rb, bool grounded, PlayerInputs input, VehicleSpeed speedData)
        {
            linearDragCheck = Mathf.Abs(input.accelInput) < 0.05 || grounded;
            float linearDragToAdd = linearDragCheck ? linearDrag : 0;

            brakingDragCheck = input.accelInput < 0 && speedData.forwardSpeed > 0;
            float brakingDragToAdd = brakingDragCheck ? brakingDrag : 0;
        
            freeWheelDragCheck = Math.Abs(input.accelInput) < 0.02f && grounded;
            float freeWheelDragToAdd = freeWheelDragCheck ? freeWheelDrag : 0;

            rb.linearDamping = linearDragToAdd + brakingDragToAdd + freeWheelDragToAdd;
        }
    }
}
