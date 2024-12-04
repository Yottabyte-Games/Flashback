using System.Collections;
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
    public Parking currentParking;
    private IEnumerator Start()
    {
        Agent = GetComponent<NavMeshAgent>();

        float waitTime = Random.Range(5f, 25f);
        yield return new WaitForSeconds(waitTime);
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
        currentParkingLot.LeaveParking(currentParking);
        Parkinglot newLot = carManager.FindNewDestination(currentParkingLot);
        Parking newParking = newLot.FindParking();
        Agent.destination = newParking.transform.position;
        currentParking = newParking;
        currentParkingLot = newLot;
    }
}