using UnityEngine;
namespace _Scripts.Fishing
{
    public class LookAtCursor : MonoBehaviour
    {
    
        void Update()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector2 direction = mousePosition - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direction); 
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
    }
}
