using NaughtyAttributes;
using System;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public enum Activity
{
    Nothing,
    Working,
    Break,
    Meeting,
}

public class OfficeWorker : Creature
{
    [field: SerializeField] public Activity activity { get; private set; }
    public bool interacting { get; private set; }
    [field: SerializeField, Expandable] public OfficeTask task { get; private set; }
    [SerializeField] GameObject taskMarker;

    [HideInInspector] public Transform officeStation;
    [HideInInspector] public ActivityRoom breakRoom;
    [HideInInspector] public ActivityRoom[] meetingRooms;

    public event Action<OfficeWorker> EndedActivity;

    WorkInteractable workInteractable;
    protected override void Awake()
    {
        base.Awake();
        workInteractable = GetComponent<WorkInteractable>();
        workInteractable.enabled = false;
    }

    protected virtual async void Start()
    {
        while (Application.isPlaying)
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
                SetDestination(officeStation);
                break;
            case Activity.Break:
                Transform breakSeat = breakRoom.RequestSeat(this);

                if (breakSeat == null)
                {
                    SetActivity(Activity.Working);
                }
                else
                {
                    SetDestination(breakSeat);
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
                    SetDestination(meetingSeat);
                }
                break;
        }
    }
    async Task IsInteracting()
    {
        while (interacting && Application.isPlaying)
        {
            await Task.Delay(10);
        }
    }
    #endregion

    #region Tasks
    public void GenerateOfficeTask()
    {
        int randomTask = UnityEngine.Random.Range(0, 2);
        switch(randomTask)
        {
            case 0:
                task = ScriptableObject.CreateInstance<FetchTask>();
                break;
            case 1:
                task = ScriptableObject.CreateInstance<CleaningTask>();
                break;
        }

        taskMarker.SetActive(true);
        workInteractable.enabled = true;
    }
    public void StartOfficeTask(Transform interactor)
    {
        if(task == null)
        {
            return;
        }

        if (task.initialized)
        {
            return;
        }

        interacting = true;
        SetDestination(interactor);
        task.InitializeTask();
        taskMarker.SetActive(false);
        workInteractable.enabled = false;
    }
    #endregion
}
