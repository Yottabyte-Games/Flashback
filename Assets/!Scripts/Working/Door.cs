using GinjaGaming.FinalCharacterController;
using Imp_Assets.GinjaGaming.FinalCharacterController.Scripts;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Working
{
    public class Door : MonoBehaviour
    {
        [SerializeField] Transform door;
        [SerializeField] List<Collider> withinDoor; 

        bool _isOpen;

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Creature creature))
            {
                if(!_isOpen)
                    OpenDoor(creature.transform);

                withinDoor.Add(other);
            } else if(other.TryGetComponent(out PlayerController player))
            {
                if (!_isOpen)
                    OpenDoor(player.transform);

                withinDoor.Add(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            withinDoor.Remove(other);

            if (withinDoor.Count == 0)
                CloseDoor();
        }

        void OpenDoor(Transform toOpenDoor)
        {
            float whatSide = toOpenDoor.position.x - transform.position.x;

            if (whatSide > 0)
            {
                door.transform.localEulerAngles = new Vector3(0.0f, -100, 0.0f);
            }
            else
            {

                door.transform.localEulerAngles = new Vector3(0.0f, 100, 0.0f);
            }

            _isOpen = true;
        }
        void CloseDoor()
        {
            door.transform.localEulerAngles = Vector3.zero;
            _isOpen = false;
        }
    }
}
