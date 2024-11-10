using UnityEngine;

namespace _Scripts.Working
{
<<<<<<< HEAD:Assets/Scenes/Working/Door.cs
    [SerializeField] Transform door;

    bool isOpen;

    void OnTriggerEnter(Collider other)
=======
    public class Door : MonoBehaviour
>>>>>>> Build:Assets/!Scripts/Working/Door.cs
    {
        [SerializeField] Transform door;

        bool _isOpen;

        private void OnTriggerEnter(Collider other)
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
        private void OnTriggerExit(Collider other)
        {
            door.transform.localEulerAngles = Vector3.zero;
        }
    }
<<<<<<< HEAD:Assets/Scenes/Working/Door.cs

    void OnTriggerExit(Collider other)
    {
        door.transform.localEulerAngles = Vector3.zero;
    }
=======
>>>>>>> Build:Assets/!Scripts/Working/Door.cs
}
