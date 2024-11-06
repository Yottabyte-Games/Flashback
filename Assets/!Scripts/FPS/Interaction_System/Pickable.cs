using UnityEngine;

namespace _Scripts.FPS.Interaction_System
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pickable : MonoBehaviour, IPickable
    {
        void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        Rigidbody rigid;

        public Rigidbody Rigid
        {
            get => rigid;
            set => rigid = value;
        }

        bool Picked
        {
            get;
            set;
        }

        public void OnHold()
        {

        }

        public void OnPickUp()
        {

        }

        public void OnRelease()
        {

        }
    }
}
