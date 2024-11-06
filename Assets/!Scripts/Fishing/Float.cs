using UnityEngine;

namespace _Scripts
{
    public class Float : MonoBehaviour
    {
        [SerializeField] float buoyancy = 20;
        Rigidbody rb;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            rb.linearVelocity /= 100;
        }

        private void OnTriggerStay(Collider other)
        {
            if (transform.position.y < other.transform.position.y)
                rb.AddForce(other.transform.up * buoyancy);
        }

    }
}
