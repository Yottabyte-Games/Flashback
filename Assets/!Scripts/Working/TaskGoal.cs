using System;
using UnityEngine;

namespace _Scripts.Working
{
    public class TaskGoal : MonoBehaviour
    {
        WorkInteractable _interactable;

        public event Action reached;

        void Start()
        {
            _interactable = GetComponent<WorkInteractable>();
            _interactable.interact.AddListener(Completed);
        }

        public void Completed(Transform transform)
        {
            reached?.Invoke();
        }
    }
}