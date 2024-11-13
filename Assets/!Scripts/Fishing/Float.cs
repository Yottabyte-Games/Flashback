using UnityEngine;

namespace _Scripts
{
    public class Float : MonoBehaviour
    {
        [SerializeField] float buoyancy = 20;
        Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (rb.isKinematic) return;
                rb.linearVelocity /= 100;
        }

        void OnTriggerStay(Collider other)
        {
            if (transform.position.y < other.transform.position.y)
                rb.AddForce(other.transform.up * buoyancy);
        }

    }
}
