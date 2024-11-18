using _Scripts.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    static Vector3 playerPosition;
    static bool firstTime = true;

    [SerializeField] Transform player;

    void Start()
    {
        if (firstTime)
        {
            playerPosition = player.position;
            firstTime = false;
        }

        player.position = playerPosition;
    }

    void OnDestroy()
    {
        playerPosition = player.position;
    }
}