using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    //[SerializeField] private ProjectileGun catScript;
    public Rigidbody rb;
    public BoxCollider coll;
    public Transform player, catContainer, fpsCam;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    private void Update()
    {
        // Check if player is in range and "E" is pressed
        Vector3 distaceToPlayer = player.position - transform.position;
        if (!equipped && distaceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !slotFull) PickUp();

        // Drop if equipped and "Q" is pressed
        if (equipped && Input.GetKeyDown(KeyCode.Q)) Drop();
    }

    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        // Make Rigidbody kinematic and BoxCollider a trigger so it can't move anymore
        rb.isKinematic = true;
        coll.isTrigger = true;

        // Enable script
        //catScript.enabled = true;
    }

    private void Drop()
    {
        equipped = false;
        slotFull = false;

        // Make Rigidbody not kinematic and BoxCollider normal so it can't move anymore
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Disable script
        // catScript.enabled = false;
    }
}
