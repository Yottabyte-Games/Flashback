using UnityEngine;
using UnityEngine.AI;

public abstract class Creature : MonoBehaviour
{
    protected NavMeshAgent agent {get; private set;}
    public bool reachedDestination
    {
        get
        {
            return !agent.hasPath || agent.velocity.sqrMagnitude == 0;
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 pos)
    {
        agent.destination = pos;
    }
}
