using System;
using UnityEngine;

public class TaskItem : MonoBehaviour
{
    public GameObject indicator;

    public event Action InteractedWith;

    void Start()
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
