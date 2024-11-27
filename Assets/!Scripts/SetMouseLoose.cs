using UnityEngine;

namespace _Scripts
{

    public class SetMouseLoose : MonoBehaviour
    {
        void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

}
