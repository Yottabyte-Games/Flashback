using NaughtyAttributes;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public enum Activity
{
    Nothing,
    Working,
    Break,
    Meeting,
    Interacting
}

public class OfficeWorker : Creature
{
    [field: SerializeField]public Activity activity { get; private set; }
    [field: SerializeField, Expandable] public OfficeTask task { get; private set; }
    [SerializeField] GameObject taskMarker;

    [HideInInspector] public Transform officeStation;
    [HideInInspector] public ActivityRoom breakRoom;
    [HideInInspector] public ActivityRoom[] meetingRooms;
    [SerializeField] Transform InteractingWith;

    public event Action<OfficeTask> NewOfficeTask;
    public event Action<OfficeTask> CompletedOfficeTask;

    public event Action<OfficeWorker> EndedActivity;

    async void Start()
    {
        SetActivity(Activity.Working);

        while (true)
        {
            await IsInteracting();

            int activity = UnityEngine.Random.Range(0, 101);

            if(task == null)
            {
                int task = UnityEngine.Random.Range(0, 16);
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
                default:
                    SetActivity(Activity.Working);
                    break;
            }

            int toDelay = UnityEngine.Random.Range(20, 60);

            await Task.Delay(toDelay * 1000);
        }
    }

    #region Activity
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

                if (meetingSeat == null)
                {
                    SetActivity(Activity.Working);
                }
                else
                {
                    SetDestination(meetingSeat.position);
                }
                break;
            case Activity.Interacting:
                break;
        }
    }
    async Task IsInteracting()
    {
        while (activity == Activity.Interacting && Application.isPlaying)
        {
            SetDestination(InteractingWith.position);

            await Task.Delay(10);
        }
    }
    #endregion

    #region Tasks
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
    public void StartOfficeTask(Transform interactor)
    {
        InteractingWith = interactor;
        SetActivity(Activity.Interacting);
        task.InitializeTask();
    }
    #endregion
}
