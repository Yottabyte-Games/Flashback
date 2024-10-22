using UnityEngine;

public class Float : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        rb.useGravity = false;
    }
    private void OnTriggerStay(Collider other)
    {
        rb.AddForce(-Physics.gravity, ForceMode.Force);
        rb.linearVelocity = rb.linearVelocity / 2;
    }
    private void OnTriggerExit(Collider other)
    {
        rb.useGravity = true;
    }
}
