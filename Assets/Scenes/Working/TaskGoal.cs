using System;
using UnityEngine;

public class TaskGoal : MonoBehaviour
{
    WorkInteractable interactable;

    public event Action reached;

    void Start()
    {
        interactable = GetComponent<WorkInteractable>();
        interactable.interact.AddListener(Completed);
    }

    public void Completed(Transform transform)
    {
        reached?.Invoke();
    }
}