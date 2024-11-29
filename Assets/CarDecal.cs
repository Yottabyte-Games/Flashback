using UnityEngine;
using UnityEngine.AI;

internal class CarDecal : MonoBehaviour
{
    [HideInInspector] public CarManager carManager;


    public NavMeshAgent Agent { get; private set; }

    float timeStopped;
    public bool ReachedDestination
    {
        get
        {
            return !Agent.hasPath || Agent.velocity.sqrMagnitude == 0;
        }
    }
    public Transform Destination { get; set; }

    [SerializeField] Transform art;
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Destination = carManager.FindNewDestination();
    }

    private void FixedUpdate()
    {
        if (ReachedDestination)
        {
            timeStopped += Time.fixedDeltaTime;

            Agent.destination = Destination.position;

            if (timeStopped > 1)
            {
                timeStopped = 0;
                Agent.Warp(carManager.FindNewDestination().position);
            }
        }
        else
        {
            timeStopped = 0;
        }
    }

    Vector3 LookDir()
    {
        if(Physics.Raycast(transform.position + transform.forward * 3 + Vector3.up * 2, -Vector3.up, out RaycastHit hit, 5))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}