using UnityEngine;

namespace Imp_Assets.Mini_First_Person_Controller.Scripts.Components
{
    public class Jump : MonoBehaviour
    {
        Rigidbody _rigidbody;
        public float jumpStrength = 2;
        public event System.Action Jumped;

        [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
        GroundCheck groundCheck;


        void Reset()
        {
            // Try to get groundCheck.
            groundCheck = GetComponentInChildren<GroundCheck>();
        }

        void Awake()
        {
            // Get rigidbody.
            _rigidbody = GetComponent<Rigidbody>();
        }

        void LateUpdate()
        {
            // Jump when the Jump button is pressed and we are on the ground.
            if (!Input.GetButtonDown("Jump")) return;
            if (groundCheck && !groundCheck.isGrounded) return;
            _rigidbody.AddForce(Vector3.up * (100 * jumpStrength));
            Jumped?.Invoke();
        }
    }
}
