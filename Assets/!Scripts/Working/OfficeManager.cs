using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

public class OfficeManager : MonoBehaviour
{
    [Header("Workers")]
    [SerializeField] GameObject bossPrefab;
    [SerializeField] GameObject officeWorkerPrefab;

    [ReadOnly] public Boss boss;
    [ReadOnly] public List<OfficeWorker> workers = new List<OfficeWorker>();

    [Header("Rooms")]
    [SerializeField] Transform enterance;
    [SerializeField] ActivityRoom[] meetingRooms;
    [SerializeField] ActivityRoom breakRoom;
    [SerializeField] Transform bossOffice;
    Cublicle[] cubicles;

    [Header("Tasks")]
    [Expandable, ReadOnly] public List<OfficeTask> officeTasks = new List<OfficeTask>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        cubicles = FindObjectsByType<Cublicle>(FindObjectsSortMode.None);

        GameObject bg = Instantiate(bossPrefab, enterance.position, enterance.rotation);
        boss = bg.GetComponent<Boss>();
        boss.breakRoom = breakRoom;
        boss.meetingRooms = meetingRooms;
        boss.officeStation = bossOffice;

        foreach (var item in cubicles)
        {
            if(Application.isPlaying)
            {
                GameObject wg = Instantiate(officeWorkerPrefab, enterance.position, enterance.rotation);

                OfficeWorker worker = wg.GetComponent<OfficeWorker>();
                worker.officeStation = item.transform;
                worker.meetingRooms = meetingRooms;
                worker.breakRoom = breakRoom;

                workers.Add(worker);

                worker.NewOfficeTask += AddOfficeTask;
                worker.CompletedOfficeTask += CompleteOfficeTask;

                await Task.Delay(1000);
            }
        }
    }

    void AddOfficeTask(OfficeTask task)
    {
        officeTasks.Add(task);
    }
    void CompleteOfficeTask(OfficeTask task)
    {
        officeTasks.Remove(task);
    }
}
