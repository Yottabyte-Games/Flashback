using UnityEngine;
using YottabyteGames.FpsScripts.Interaction_System;
using InteractableBase = YottabyteGames.Scripts.Interaction_System.InteractableBase;

namespace YottabyteGames.FpsScripts.Interactables
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
