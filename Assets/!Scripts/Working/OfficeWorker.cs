using NaughtyAttributes;
using System;
using System.Collections;
using UnityEngine;

public enum Activity
{
    Nothing,
    Working,
    Break,
    Meeting
}

public class OfficeWorker : Creature
{
    [field: SerializeField]public Activity activity { get; private set; }
    [field: SerializeField, Expandable] public OfficeTask task { get; private set; }
    [SerializeField] GameObject taskMarker;

    [HideInInspector] public Transform officeStation;
    [HideInInspector] public ActivityRoom[] meetingRooms;
    [HideInInspector] public ActivityRoom breakRoom;

    public event Action<OfficeTask> NewOfficeTask;
    public event Action<OfficeTask> CompletedOfficeTask;

    public event Action<OfficeWorker> EndedActivity;

    IEnumerator Start()
    {
        SetDestination(officeStation.position);

        while (true)
        {
            SetActivity(Activity.Working);

            int activity = UnityEngine.Random.Range(0, 101);

            if(task == null)
            {
                int task = UnityEngine.Random.Range(0, 6);
                if (task == 0)
                {
                    GenerateOfficeTask();
                }
            }

            switch (activity)
            {
                case > 85: // Meeting
                    SetActivity(Activity.Meeting);
                    break;
                case > 60: // Break
                    SetActivity(Activity.Break);
                    break;
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(20, 60));
        }
    }

    public void EndActivity()
    {
        EndedActivity?.Invoke(this);   
    }
    public void SetActivity(Activity activity)
    {
        EndActivity();

        this.activity = activity;

        UpdateActivity();
    }
    public void GenerateOfficeTask()
    {
        task = ScriptableObject.CreateInstance<OfficeTask>();

        NewOfficeTask?.Invoke(task);
        taskMarker.SetActive(true);
    }
    [Button]
    public void FisnishOfficeTask()
    {
        task = null;

        CompletedOfficeTask?.Invoke(task);
        taskMarker.SetActive(false);
    }

    void UpdateActivity()
    {
        switch (activity)
        {
            case Activity.Nothing:
                break;
            case Activity.Working:
                SetDestination(officeStation.position);
                break;
            case Activity.Break:
                Transform breakSeat = breakRoom.RequestSeat(this);

                if (breakSeat == null)
                {
                    SetActivity(Activity.Working);
                }
                else
                {
                    SetDestination(breakSeat.position);
                }
                break;
            case Activity.Meeting:

                Transform meetingSeat = meetingRooms[UnityEngine.Random.Range(0, meetingRooms.Length)].RequestSeat(this);

                if(meetingSeat == null)
                {
                    SetActivity(Activity.Working);
                }
                else
                {
                    SetDestination(meetingSeat.position);
                }
                break;
        }
    }
}
