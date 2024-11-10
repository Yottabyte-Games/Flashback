using _Scripts.FPS.Interaction_System;
using UnityEngine;

namespace VHS
{
    public class ActivateDialogueInteractable : InteractableBase
    {
        [SerializeField] InteractionController interactionController;

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
