using UnityEngine;
using UnityEngine.Events;
namespace _Scripts
{
    public class TriggerDialogueOnExit : MonoBehaviour
    {
        [SerializeField] UnityEvent eventOnExit;
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Trigger dialogue
                eventOnExit.Invoke();
            }
        }
    }
}
