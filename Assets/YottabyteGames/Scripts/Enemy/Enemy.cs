using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject Player;

    NavMeshAgent NMA;

    void Start()
    {
        NMA = GetComponent<NavMeshAgent>();
        //Player = GameObject.()
    }

    // Update is called once per frame
    void Update()
    {
        //NMA.destination = Player.
    }
}
