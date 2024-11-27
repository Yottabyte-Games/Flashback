using System;
using UnityEngine;
using UnityEngine.Rendering;

public class CarManager : MonoBehaviour
{
    [SerializeField] int carsToSpawn;
    [SerializeField] CarDecal[] CarPrefabs;
    [SerializeField] Transform[] CarSpawns;

    private void Start()
    {
        for (int i = 0; i < carsToSpawn; i++)
        {
            NewCar();
        }

        InvokeRepeating(nameof(NewCar), 5, 5);
    }

    void NewCar()
    {
        Transform spawn = FindNewDestination();
        var current = Instantiate(CarPrefabs[UnityEngine.Random.Range(0, CarPrefabs.Length)], spawn.position, spawn.rotation);
        current.carManager = this;
    }
    internal Transform FindNewDestination()
    {
        return CarSpawns[UnityEngine.Random.Range(0, CarSpawns.Length)]; 
    }
}
