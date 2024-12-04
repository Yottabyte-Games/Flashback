using UnityEngine;

public class Parkinglot : MonoBehaviour
{
    [SerializeField] Parking[] parkings; 
    
    public Parking FindParking()
    {
        return parkings[Random.Range(0, parkings.Length)];
    }
}
