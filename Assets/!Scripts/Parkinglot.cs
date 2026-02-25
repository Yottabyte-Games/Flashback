using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Parkinglot : MonoBehaviour
{
    [SerializeField] Parking[] parkings;
    [SerializeField] List<CarDecal> carsInLot = new();

    IEnumerator Start()
    {
        float waitTime = Random.Range(2, 10);
        yield return new WaitForSeconds(waitTime);

        KickRandomCar();

        while(true)
        {
            if (carsInLot.Count > 0)
            {
                int divisor = carsInLot.Count / 3;
                if (divisor == 0) divisor = 1;
                waitTime = Random.Range(4, 60 / divisor);
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                waitTime = 10;
                yield return new WaitForSeconds(waitTime);
            }
            

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
    async void KickRandomCar()
    {
        CarDecal car = await RandomCar();
        if (car is not null)
        {
            car.NewJourney();
        }
    }
    async Task<CarDecal> RandomCar()
    {
        if (carsInLot.Count <= 0)
        {
            Debug.LogWarning("no cars in lot");
            return null;
        }

        CarDecal car = carsInLot[Random.Range(0, carsInLot.Count)];
        while (!car.ReachedDestination)
        {
            car = carsInLot[Random.Range(0, carsInLot.Count)];

            await Task.Delay(10);
        }

        return car;
    }
}
