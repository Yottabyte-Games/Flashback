using UnityEngine;

namespace YottabyteGames
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
