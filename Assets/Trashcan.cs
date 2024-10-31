using UnityEngine;

public class Trashcan : MonoBehaviour
{
    public void ThrowAwayTrash(Transform player)
    {
        Holding held = player.GetComponent<Holding>();
        if (held.heldItem == null) return;
        if (!held.heldItem.TryGetComponent(out TrashItem trash)) return;

        Destroy(held.heldItem);
    }
}
