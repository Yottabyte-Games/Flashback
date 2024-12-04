using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CarManager : MonoBehaviour
{
    [SerializeField] int carsToSpawn;
    [SerializeField] CarDecal[] CarPrefabs;
    [SerializeField] Parkinglot[] ParkingLots;

    private IEnumerator Start()
    {
        for (int i = 0; i < carsToSpawn; i++)
        {
            NewCar();
            yield return null;
        }
    }

    void NewCar()
    {
        Parkinglot parkinglot = FindNewDestination();
        Parking parking = parkinglot.FindParking();
        Transform spawn = parking.transform;
        var current = Instantiate(CarPrefabs[UnityEngine.Random.Range(0, CarPrefabs.Length)], spawn.position, spawn.rotation);
        current.carManager = this;
        current.currentParkingLot = parkinglot;
        current.currentParking = parking;
    }
    public Parkinglot FindNewDestination()
    {
        return ParkingLots[UnityEngine.Random.Range(0, ParkingLots.Length)];
    }
    public Parkinglot FindNewDestination(Parkinglot ignoreLot)
    {
        Parkinglot current = FindNewDestination();
        while (current == ignoreLot)
        {
            current = FindNewDestination();
        }

        return current;
    }
}
