using _Scripts.FPS.Interaction_System;

namespace _Scripts.FPS.Interactables
{    
    public class DestroyInteractable : InteractableBase
    {

        public override void OnInteract()
        {
            base.OnInteract();

            Destroy(gameObject);
        }
    }
}
