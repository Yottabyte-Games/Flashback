using UnityEngine;

public class Parkinglot : MonoBehaviour
{
    [SerializeField] Parking[] parkings; 
    
    public Parking FindParking()
    {
        Parking current = parkings[Random.Range(0, parkings.Length)];
        while (current.isTaken)
        {
            current = parkings[Random.Range(0, parkings.Length)];
        }
        current.isTaken = true;
        return current;
    }
    public void LeaveParking(Parking parking)
    {
        parking.isTaken = false;
    }
}
