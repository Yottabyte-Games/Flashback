using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform door;

    bool isOpen;

    private void OnTriggerEnter(Collider other)
    {
        float whatSide = other.transform.position.x - transform.position.x;
        
        if(whatSide > 0)
        {

            door.transform.eulerAngles = new Vector3(0.0f, -100, 0.0f);
        } else
        {

            door.transform.eulerAngles = new Vector3(0.0f, 100, 0.0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        door.transform.eulerAngles = Vector3.zero;
    }
}
