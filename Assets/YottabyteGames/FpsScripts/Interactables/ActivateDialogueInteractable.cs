using UnityEngine;
using YottabyteGames.FpsScripts.Interaction_System;

namespace YottabyteGames.FpsScripts.Interactables
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
