using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - transform.position;
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        print(angle);   
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
