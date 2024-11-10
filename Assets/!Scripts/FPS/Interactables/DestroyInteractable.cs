using _Scripts.FPS.Interaction_System;

namespace VHS
{
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
