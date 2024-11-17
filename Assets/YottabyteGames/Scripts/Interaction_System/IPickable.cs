using UnityEngine;

namespace YottabyteGames
{    
    public interface IPickable
    {
        Rigidbody Rigid {get;set;}

        void OnPickUp();
        void OnHold();
        void OnRelease();
    }
}
