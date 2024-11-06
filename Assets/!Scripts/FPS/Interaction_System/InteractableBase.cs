using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.FPS.Interaction_System
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        #region Variables    
            [Space,Header("Interactable Settings")]

            [SerializeField]
            bool holdInteract = true;
            [ShowIf("holdInteract")][SerializeField]
            float holdDuration = 1f;
            
            [Space] 
            [SerializeField]
            bool multipleUse = false;
            [SerializeField] bool isInteractable = true;

            [SerializeField] string tooltipMessage = "interact";
        #endregion

        #region Properties    
            public float HoldDuration => holdDuration; 

            public bool HoldInteract => holdInteract;
            public bool MultipleUse => multipleUse;
            public bool IsInteractable => isInteractable;

            public string TooltipMessage => tooltipMessage;
        #endregion

        #region Methods
        public virtual void OnInteract()
            {
                Debug.Log("INTERACTED: " + gameObject.name);
            }
        #endregion
    }
}
