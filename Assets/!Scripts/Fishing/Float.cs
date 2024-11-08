using UnityEngine;

namespace _Scripts.Fishing
{
    public class Float : MonoBehaviour
    {
        Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            _rb.useGravity = false;
        }

        void OnTriggerStay(Collider other)
        {
            _rb.AddForce(-Physics.gravity, ForceMode.Force);
            _rb.linearVelocity = _rb.linearVelocity / 2;
        }

        void OnTriggerExit(Collider other)
        {
            _rb.useGravity = true;
        }
    }
}
