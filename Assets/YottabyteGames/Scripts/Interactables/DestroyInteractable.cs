using UnityEngine;
using YottabyteGames.Scripts.Interaction_System;

namespace YottabyteGames.Scripts.Interactables
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
