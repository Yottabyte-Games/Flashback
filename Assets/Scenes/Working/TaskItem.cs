using System;
using UnityEngine;

namespace _Scripts.Working
{
<<<<<<< HEAD:Assets/Scenes/Working/TaskItem.cs
    public GameObject indicator;

    public event Action InteractedWith;

    void Start()
=======
    public class TaskItem : MonoBehaviour
>>>>>>> Build:Assets/!Scripts/Working/TaskItem.cs
    {
        public GameObject indicator;

        public event Action InteractedWith;
        private void Start()
        {
            indicator = Instantiate(indicator, transform);
        }

        public void Interact(Transform heldBy)
        {
            if(heldBy.GetComponent<Holding>().HoldItem(gameObject))
            {
                indicator.SetActive(false);
                InteractedWith?.Invoke();
            }
        }
    }
}
