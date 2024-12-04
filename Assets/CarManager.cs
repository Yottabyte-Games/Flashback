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
            yield return new WaitForSeconds(2f);
        }
    }

    void NewCar()
    {
        Parkinglot parkinglot = FindNewDestination();
        Transform spawn = FindNewDestination().FindParking().transform;
        var current = Instantiate(CarPrefabs[UnityEngine.Random.Range(0, CarPrefabs.Length)], spawn.position, spawn.rotation);
        current.carManager = this;
        current.currentParkingLot = parkinglot;
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
