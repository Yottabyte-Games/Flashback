using GinjaGaming.FinalCharacterController;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    [SerializeField] Vector3 axis = Vector3.one;

    Transform player;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    void Update()
    {
        transform.LookAt(player.position);

        transform.localEulerAngles = new Vector3 (
            transform.localEulerAngles.x * axis.x,
            transform.localEulerAngles.y * axis.y, 
            transform.localEulerAngles.z * axis.z);
    }
}
