using System;
using UnityEngine;
using NaughtyAttributes;

public class Door : MonoBehaviour
{
    [SerializeField] Transform door;

    bool isOpen;

    void OnTriggerEnter(Collider other)
    {
        float whatSide = other.transform.position.x - transform.position.x;
        
        if(whatSide > 0)
        {
            door.transform.localEulerAngles = new Vector3(0.0f, -100, 0.0f);
        } else
        {

            door.transform.localEulerAngles = new Vector3(0.0f, 100, 0.0f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        door.transform.localEulerAngles = Vector3.zero;
    }
}
