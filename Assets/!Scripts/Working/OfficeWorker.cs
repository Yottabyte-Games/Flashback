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
    [HideInInspector] public Transform[] meetingRooms;
    [HideInInspector] public Transform breakRoom;

    public event Action<OfficeTask> NewOfficeTask;
    public event Action<OfficeTask> CompletedOfficeTask;

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
                case > 90: // Meeting
                    SetActivity(Activity.Meeting);
                    break;
                case > 80: // Break
                    SetActivity(Activity.Break);
                    break;
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(20, 60));
        }
    }

    public void SetActivity(Activity activity)
    {
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
                SetDestination(breakRoom.position);
                break;
            case Activity.Meeting:
                SetDestination(meetingRooms[UnityEngine.Random.Range(0, meetingRooms.Length)].position);
                break;
        }
    }
}
