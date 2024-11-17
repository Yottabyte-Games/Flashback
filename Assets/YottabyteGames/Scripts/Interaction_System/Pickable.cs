using UnityEngine;

namespace YottabyteGames
{
    [RequireComponent(typeof(Rigidbody))]
    public class Pickable : MonoBehaviour, IPickable
    {
        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        private Rigidbody rigid;

        public Rigidbody Rigid
        {
            get => rigid;
            set => rigid = value;
        }

        private bool Picked
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
