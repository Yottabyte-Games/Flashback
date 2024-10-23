using System.Collections.Generic;
using UnityEngine;

namespace Imp_Assets.Mini_First_Person_Controller.Scripts
{
    public class FirstPersonMovement : MonoBehaviour
    {
        public float speed = 5;

        [Header("Running")]
        public bool canRun = true;
        public bool IsRunning { get; private set; }
        public float runSpeed = 9;
        public KeyCode runningKey = KeyCode.LeftShift;

        Rigidbody _rigidbody;
        /// <summary> Functions to override movement speed. Will use the last added override. </summary>
        public List<System.Func<float>> SpeedOverrides = new List<System.Func<float>>();



        void Awake()
        {
            // Get the rigidbody on this.
            _rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            // Update IsRunning from input.
            IsRunning = canRun && Input.GetKey(runningKey);

            // Get targetMovingSpeed.
            var targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (SpeedOverrides.Count > 0)
            {
                targetMovingSpeed = SpeedOverrides[SpeedOverrides.Count - 1]();
            }

            // Get targetVelocity from input.
            var targetVelocity = new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

            // Apply movement.
            _rigidbody.linearVelocity = transform.rotation * new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.y);
        }
    }
}