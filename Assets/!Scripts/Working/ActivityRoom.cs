using System.Collections.Generic;
using System.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Working
{
    public class ActivityRoom : MonoBehaviour
    {
        [SerializeField] bool findRandomSeats;
        public List<WorkerPosition> seats = new List<WorkerPosition>();

        [Button]
        async void FindAllSeats()
        {
            seats.Clear();

            Seat[] s = GetComponentsInChildren<Seat>();

            for (int i = 0; i < s.Length; i++)
            {
                seats.Add(new WorkerPosition() { transform = s[i].transform });

                await Task.Delay(10);
            }

        }

        public void LeaveSeat(OfficeWorker worker)
        {
            for (int i = 0; i < seats.Count; i++)
            {
                if (seats[i].workerInPosition == worker)
                {
                    seats[i].workerInPosition = null;
                    break;
                }
            }
        }
    
        public Transform RequestSeat(OfficeWorker worker)
        {
            for (int i = 0; i < seats.Count; i++)
            {
                if(!findRandomSeats)
                {
                    if (CheckSeat(seats[i]))
                    {
                        seats[i].SetWorker(worker);
                        worker.EndedActivity += LeaveSeat;

                        return seats[i].transform;
                    }
                } 
                else
                {
                    int seatInQuestion = Random.Range(0, seats.Count);

                    if (CheckSeat(seats[seatInQuestion]))
                    {
                        seats[seatInQuestion].SetWorker(worker);
                        worker.EndedActivity += LeaveSeat;

                        return seats[seatInQuestion].transform;
                    }
                }
            }

            return null;
        }

        bool CheckSeat(WorkerPosition seat)
        {
            if (seat.workerInPosition == null)
            {
                return true;
            }
            return false;
        }
    }
}
