using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class CarManager : MonoBehaviour
{
    [SerializeField] int carsToSpawn;
    [SerializeField] CarDecal[] CarPrefabs;
    [SerializeField] Parkinglot[] ParkingLots;

    [SerializeField] Transform tempSpawn;

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
        var current = Instantiate(CarPrefabs[UnityEngine.Random.Range(0, CarPrefabs.Length)], tempSpawn.position, tempSpawn.rotation);

        Parkinglot parkinglot = FindNewDestination();
        Parking parking = parkinglot.FindParking(current);
        Transform spawn = parking.transform;

        current.Agent.Warp(spawn.position);
        current.transform.rotation = spawn.rotation;
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
