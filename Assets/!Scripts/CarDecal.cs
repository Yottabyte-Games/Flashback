using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CarDecal : MonoBehaviour
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
    public Parking currentParking;
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    public void NewJourney()
    {
        currentParkingLot.LeaveParking(currentParking, this);
        Parkinglot newLot = carManager.FindNewDestination(currentParkingLot);
        Parking newParking = newLot.FindParking(this);
        Agent.destination = newParking.transform.position;
        currentParking = newParking;
        currentParkingLot = newLot;
    }
}