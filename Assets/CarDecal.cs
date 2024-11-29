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
        Agent.destination = Destination.position;
        //art.LookAt(LookDir());

        if (ReachedDestination)
        {
            timeStopped += Time.fixedDeltaTime;

            if (timeStopped > 2)
                Destroy(gameObject);
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