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

    public Parkinglot currentParkingLot;
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        NewJourney();
    }

    private void FixedUpdate()
    {
        if (ReachedDestination)
        {
            timeStopped += Time.fixedDeltaTime;

            if (timeStopped > 25)
            {
                NewJourney();

                timeStopped = 0;
            }
        }
        else
        {
            timeStopped = 0;
        }
    }

    void NewJourney()
    {
        Parkinglot newLot = carManager.FindNewDestination(currentParkingLot);

        Agent.destination = newLot.FindParking().transform.position;
        currentParkingLot = newLot;
    }
}