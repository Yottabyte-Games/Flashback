using _Scripts.FPS.Interaction_System;
using UnityEngine;

namespace VHS
{
    public class DestroyInteractable : InteractableBase
    {
        [SerializeField] GameObject cat;

        public override void OnInteract()
        {
            base.OnInteract();
            Debug.Log("WEAPON EQUIPED: " + gameObject.name);

            Destroy(gameObject);
            cat.SetActive(true);
        }
    }
}
