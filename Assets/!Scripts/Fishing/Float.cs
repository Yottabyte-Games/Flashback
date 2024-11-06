using UnityEngine;

namespace _Scripts.Fishing
{
    public class Float : MonoBehaviour
    {
        Rigidbody _rb;
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }
        private void OnTriggerEnter(Collider other)
        {
            _rb.useGravity = false;
        }
        private void OnTriggerStay(Collider other)
        {
            _rb.AddForce(-Physics.gravity, ForceMode.Force);
            _rb.linearVelocity = _rb.linearVelocity / 2;
        }
        private void OnTriggerExit(Collider other)
        {
            _rb.useGravity = true;
        }
    }
}
