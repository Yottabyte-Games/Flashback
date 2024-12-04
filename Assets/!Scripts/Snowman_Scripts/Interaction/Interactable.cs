using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Snowman_Scripts.Interaction
{
    public class Interactable : MonoBehaviour
    {
        Rigidbody _rb;
        public UnityEvent onInteract;
        Outline _outline;
        public bool canInteract = true;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _outline = GetComponent<Outline>();
            //Disable Outline
        }

        public void Interact()
        {
            if(canInteract)
            {
                _rb.isKinematic = true;
                gameObject.SetActive(false);
                onInteract?.Invoke();   
            }
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
        }

        public void EnableOutline()
        {
            //Debug.Log("Enabled Outline");
            _outline.enabled = true;
        }

        public void DisableInteraction()
        {
            canInteract = false;
            _rb.isKinematic = true;
        }
    }
}
