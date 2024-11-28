using UnityEngine;
using UnityEngine.AI;

internal class CarDecal : MonoBehaviour
{
    [HideInInspector] public CarManager carManager;


    public NavMeshAgent agent;

    float timeStopped;
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

        if (ReachedDestination)
        {
            timeStopped += Time.fixedDeltaTime;

            if (timeStopped > 5)
                Destroy(gameObject);
        }
    }
}