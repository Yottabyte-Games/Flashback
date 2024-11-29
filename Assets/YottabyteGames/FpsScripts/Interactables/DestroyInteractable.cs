using UnityEngine;
using YottabyteGames.FpsScripts.Interaction_System;

namespace YottabyteGames.FpsScripts.Interactables
{
    public class DestroyInteractable : InteractableBase
    {
        [SerializeField] GameObject cat;
        [SerializeField] GameObject spawner = null;

        public override void OnInteract()
        {
            base.OnInteract();
            Debug.Log("WEAPON EQUIPED: " + gameObject.name);

            Destroy(gameObject);
            cat.SetActive(true);
            spawner.SetActive(true);
        }
    }
}
