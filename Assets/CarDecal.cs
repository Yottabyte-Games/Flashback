using UnityEngine;
using UnityEngine.AI;

internal class CarDecal : MonoBehaviour
{
    [HideInInspector] public CarManager carManager;


    public NavMeshAgent agent;
    public bool ReachedDestination
    {
        get
        {
            return !agent.hasPath || agent.velocity.sqrMagnitude == 0;
        }
    }
    public Transform Destination { get; set; }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Destination = carManager.FindNewDestination();
    }

    private void FixedUpdate()
    {
        agent.destination = Destination.position;

        if(ReachedDestination)
        {
            Destination = carManager.FindNewDestination();
        }
    }
}