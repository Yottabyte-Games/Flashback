using UnityEngine;

namespace _Scripts.FPS.Interaction_System
{    
    public interface IPickable
    {
        Rigidbody Rigid {get;set;}

        void OnPickUp();
        void OnHold();
        void OnRelease();
    }
}
