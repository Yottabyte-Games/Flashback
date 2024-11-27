using UnityEngine;

namespace YottabyteGames.Scripts.Interaction_System
{    
    public interface IPickable
    {
        Rigidbody Rigid {get;set;}

        void OnPickUp();
        void OnHold();
        void OnRelease();
    }
}
