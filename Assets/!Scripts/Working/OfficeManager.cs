using System.Threading.Tasks;
using UnityEngine;

public class OfficeManager : MonoBehaviour
{
    [SerializeField] Transform enterance;

    [SerializeField] GameObject officeWorker;

    Cublicle[] cubicles;


    [SerializeField] Transform meetingRoom;
    [SerializeField] Transform breakRoom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        cubicles = FindObjectsByType<Cublicle>(FindObjectsSortMode.None);

        foreach (var item in cubicles)
        {
            if(Application.isPlaying)
            {
                GameObject current = Instantiate(officeWorker, enterance.position, enterance.rotation);

                OfficeWorker worker = current.GetComponent<OfficeWorker>();
                worker.officeStation = item.transform;
                worker.meetingRoom = meetingRoom;
                worker.breakRoom = breakRoom;

                await Task.Delay(1000);
            }
        }
    }
}
