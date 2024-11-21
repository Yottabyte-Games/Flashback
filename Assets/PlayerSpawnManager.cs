using _Scripts.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    static Vector3 spawnPosition;
    static bool firstTime = true;

    [SerializeField] Transform player;

    void Start()
    {
        if (firstTime)
        {
            spawnPosition = player.position;
            firstTime = false;
        }

        player.position = spawnPosition;
    }

    void OnDestroy()
    {
        spawnPosition = player.position;
    }
}