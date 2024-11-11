using _Scripts.FPS.Interaction_System;
using UnityEngine;

namespace _Scripts.FPS.Interactables
{
    public class ActivateDialogueInteractable : InteractableBase
    {
        [SerializeField] private InteractionController interactionController;

        public override void OnInteract()
        {
            base.OnInteract();

            if (interactionController.layer == LayerMask.NameToLayer("Dialogue")) 
            {
                Debug.Log("Dialogue UI activated");
            }
        }
    }
}
