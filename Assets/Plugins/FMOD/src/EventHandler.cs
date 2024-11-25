using UnityEngine;
using UnityEngine.EventSystems;

namespace FMODUnity
{
    public abstract class EventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public string CollisionTag = "";

        protected virtual void Start()
        {
            HandleGameEvent(EmitterGameEvent.ObjectStart);
        }

        protected virtual void OnDestroy()
        {
            HandleGameEvent(EmitterGameEvent.ObjectDestroy);
        }

        void OnEnable()
        {
            HandleGameEvent(EmitterGameEvent.ObjectEnable);
        }

        void OnDisable()
        {
            HandleGameEvent(EmitterGameEvent.ObjectDisable);
        }

        #if UNITY_PHYSICS_EXIST
        void OnTriggerEnter(Collider other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag) || (other.attachedRigidbody && other.attachedRigidbody.CompareTag(CollisionTag)))
            {
                HandleGameEvent(EmitterGameEvent.TriggerEnter);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag) || (other.attachedRigidbody && other.attachedRigidbody.CompareTag(CollisionTag)))
            {
                HandleGameEvent(EmitterGameEvent.TriggerExit);
            }
        }
        #endif

        #if UNITY_PHYSICS2D_EXIST
        void OnTriggerEnter2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
            {
                HandleGameEvent(EmitterGameEvent.TriggerEnter2D);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (string.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
            {
                HandleGameEvent(EmitterGameEvent.TriggerExit2D);
            }
        }
        #endif

        void OnCollisionEnter()
        {
            HandleGameEvent(EmitterGameEvent.CollisionEnter);
        }

        void OnCollisionExit()
        {
            HandleGameEvent(EmitterGameEvent.CollisionExit);
        }

        void OnCollisionEnter2D()
        {
            HandleGameEvent(EmitterGameEvent.CollisionEnter2D);
        }

        void OnCollisionExit2D()
        {
            HandleGameEvent(EmitterGameEvent.CollisionExit2D);
        }

        void OnMouseEnter()
        {
            HandleGameEvent(EmitterGameEvent.ObjectMouseEnter);
        }

        void OnMouseExit()
        {
            HandleGameEvent(EmitterGameEvent.ObjectMouseExit);
        }

        void OnMouseDown()
        {
            HandleGameEvent(EmitterGameEvent.ObjectMouseDown);
        }

        void OnMouseUp()
        {
            HandleGameEvent(EmitterGameEvent.ObjectMouseUp);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HandleGameEvent(EmitterGameEvent.UIMouseEnter);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HandleGameEvent(EmitterGameEvent.UIMouseExit);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            HandleGameEvent(EmitterGameEvent.UIMouseDown);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            HandleGameEvent(EmitterGameEvent.UIMouseUp);
        }

        protected abstract void HandleGameEvent(EmitterGameEvent gameEvent);
    }
}
