using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterEvent : MonoBehaviour
{
    public UnityEvent onTriggerEnterEvent;

    void OnTriggerEnter(Collider other)
    {
        onTriggerEnterEvent.Invoke();
    }
}
