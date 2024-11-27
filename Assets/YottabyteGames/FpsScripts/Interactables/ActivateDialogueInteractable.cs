using UnityEngine;
using YottabyteGames.FpsScripts.Interaction_System;
using InteractableBase = YottabyteGames.Scripts.Interaction_System.InteractableBase;
using InteractionController = YottabyteGames.Scripts.Interaction_System.InteractionController;

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
