using UnityEngine;

namespace _Scripts.FPS.Interaction_System
{    
    [CreateAssetMenu(fileName = "Interaction Data", menuName = "InteractionSystem/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        InteractableBase m_interactable;

        public InteractableBase Interactable
        {
            get => m_interactable;
            set => m_interactable = value;
        }

        public void Interact()
        {
            m_interactable.OnInteract();
            ResetData();
        }

        public bool IsSameInteractable(InteractableBase _newInteractable) => m_interactable == _newInteractable;
        public bool IsEmpty() => m_interactable == null;
        public void ResetData() => m_interactable = null;

    }
}
