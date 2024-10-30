using System;
using UnityEngine;

public class TaskItem : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    Transform hand;

    public event Action InteractedWith;
    private void OnEnable()
    {
        indicator = Instantiate(indicator, transform);
        hand = GameObject.Find("Held Object").transform;
    }

    public void Interact()
    {
        HoldItem();
        InteractedWith?.Invoke();
    }
    public void HoldItem()
    {
        indicator.SetActive(false);
        transform.parent = hand.transform;
        transform.localPosition = Vector3.zero;
    }
}
