using UnityEngine;
using UnityEngine.AI;
namespace _Scripts.Enemy
{
    public class Enemy : MonoBehaviour
    {
        GameObject Player;

        NavMeshAgent NMA;

        void Start()
        {
            NMA = GetComponent<NavMeshAgent>();
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            NMA.destination = Player.transform.position;    
        }
    }
}
