using UnityEngine;

namespace Plugins.Dreamteck.Scripts.Interaction_System
{    
    [CreateAssetMenu(fileName = "Interaction Data", menuName = "InteractionSystem/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        private InteractableBase m_interactable;

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
