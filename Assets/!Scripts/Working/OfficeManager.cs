using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Working
{
    public class OfficeManager : MonoBehaviour
    {
        [Header("Workers")]
        [SerializeField] GameObject bossPrefab;
        [SerializeField] GameObject officeWorkerPrefab;
        private string[] officeWorkerNames = 
        {
            "Alice", "Bjorn", "Pål", "Dagny", "Eirik",
            "Frank", "Gunnar", "Hanne", "Ingrid", "Jakob",
            "Kari", "Lars", "Mona", "Nils", "Olav",
            "Pia", "Olaf", "Ragnar", "Solveig", "Tor",
            "Unni", "Vidar", "Wendy", "Sander", "Ylva",
            "Per", "Arne", "Bente", "Christian", "Dag"
        };

        [ReadOnly] public Boss boss;
        [ReadOnly] public List<OfficeWorker> workers = new List<OfficeWorker>();

        [Header("Rooms")]
        [SerializeField] Transform enterance;
        [SerializeField] ActivityRoom[] meetingRooms;
        [SerializeField] ActivityRoom breakRoom;
        [SerializeField] Transform bossOffice;
        Cublicle[] _cubicles;

        [Header("Other")]
        [SerializeField] TaskManager taskManager;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        async void Start()
        {
            _cubicles = FindObjectsByType<Cublicle>(FindObjectsSortMode.None);

            GameObject bg = Instantiate(bossPrefab, enterance.position, enterance.rotation);
            
            int startIndex = bg.name.IndexOf("(Clone)", StringComparison.Ordinal);
            bg.name = bg.name.Remove(startIndex).Trim();
            
            boss = bg.GetComponent<Boss>();
            boss.breakRoom = breakRoom;
            boss.meetingRooms = meetingRooms;
            boss.officeStation = bossOffice;


            for (int i = 0; i < _cubicles.Length; i++)
            {
                if (!Application.isPlaying) return;

                GameObject wg = Instantiate(officeWorkerPrefab, enterance.position, enterance.rotation);
                
                wg.name = officeWorkerNames[i];
                
                OfficeWorker worker = wg.GetComponent<OfficeWorker>();
                worker.officeStation = _cubicles[i].transform;
                worker.meetingRooms = meetingRooms;
                worker.breakRoom = breakRoom;

                workers.Add(worker);

                await Task.Delay(1000);
            }
        }
    }
}
