using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Parkinglot : MonoBehaviour
{
    [SerializeField] Parking[] parkings;
    [SerializeField] List<CarDecal> carsInLot = new();

    private IEnumerator Start()
    {
        float waitTime = Random.Range(2, 10);
        yield return new WaitForSeconds(waitTime);

        KickRandomCar();

        while(true)
        {
            waitTime = Random.Range(4, 60 / carsInLot.Count);
            yield return new WaitForSeconds(waitTime);

            KickRandomCar();
        }
    }

    public Parking FindParking(CarDecal car)
    {
        Parking current = parkings[Random.Range(0, parkings.Length)];
        while (current.isTaken)
        {
            current = parkings[Random.Range(0, parkings.Length)];
        }
        carsInLot.Add(car);
        current.isTaken = true;
        return current;
    }
    public void LeaveParking(Parking parking, CarDecal car)
    {
        carsInLot.Remove(car);
        parking.isTaken = false;
    }
    void KickRandomCar()
    {
        if (RandomCar(out CarDecal car))
        {
            car.NewJourney();
        }
    }
    bool RandomCar(out CarDecal car)
    {
        if(carsInLot.Count > 0)
        {
            car = carsInLot[Random.Range(0, carsInLot.Count)];
            return true;
        }

        Debug.LogWarning("no cars in lot");
        car = null;
        return false;
    }
}
