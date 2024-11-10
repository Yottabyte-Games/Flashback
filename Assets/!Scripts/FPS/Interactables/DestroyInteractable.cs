using _Scripts.FPS.Interaction_System;

<<<<<<< HEAD:Assets/!Scripts/FPS/Interactables/DestroyInteractable.cs
namespace _Scripts.FPS.Interactables
{    
=======
namespace VHS
{
>>>>>>> Therapy-FPS:Assets/YottabyteGames/FirstPersonController/Assets/Scripts/Interactables/DestroyInteractable.cs
    public class DestroyInteractable : InteractableBase
    {
        [SerializeField] private GameObject cat;

        public override void OnInteract()
        {
            base.OnInteract();
            Debug.Log("WEAPON EQUIPED: " + gameObject.name);

            Destroy(gameObject);
            cat.SetActive(true);
        }
    }
}
