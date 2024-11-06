using System;
using UnityEngine;

namespace _Scripts.Working
{
    public class TaskGoal : MonoBehaviour
    {
        WorkInteractable interactable;

        public event Action reached;
        private void Start()
        {
            interactable = GetComponent<WorkInteractable>();
            interactable.interact.AddListener(Completed);
        }

        public void Completed(Transform transform)
        {
            reached?.Invoke();
        }
    }
}