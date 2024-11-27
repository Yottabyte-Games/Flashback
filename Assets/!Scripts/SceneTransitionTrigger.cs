using UnityEngine;
using UnityEngine.Events;

namespace _Scripts
{

    public class OnTriggerEnterEvent : MonoBehaviour
    {
        public UnityEvent onTriggerEnterEvent;

        void OnTriggerEnter(Collider other)
        {
            onTriggerEnterEvent.Invoke();
        }
    }

}
