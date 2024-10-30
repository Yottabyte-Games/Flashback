using System;
using UnityEngine;

public class TaskItem : MonoBehaviour
{
    [SerializeField] GameObject indicator;

    public event Action InteractedWith;
    private void OnEnable()
    {
        Instantiate(indicator, transform);
    }

    public void Interact()
    {
        InteractedWith?.Invoke();
    }
}
