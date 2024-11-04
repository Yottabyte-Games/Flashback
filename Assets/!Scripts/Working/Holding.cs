using UnityEngine;
using UnityEngine.XR;

public class Holding : MonoBehaviour
{
    [SerializeField] Transform hand;
    public GameObject heldItem { get; private set; }

    public bool HoldItem(GameObject item)
    {
        if (heldItem != null) return false;

        heldItem = item;

        item.transform.parent = hand.transform;
        item.transform.localPosition = Vector3.zero;
        item.gameObject.layer = 7;

        for (int i = 0; i < item.transform.childCount; i++)
        {
            item.transform.GetChild(i).gameObject.layer = 7;
        }

        return true;
    }
}
