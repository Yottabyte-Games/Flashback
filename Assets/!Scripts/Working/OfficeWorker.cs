using System;
using System.Threading.Tasks;
using _Scripts.Working.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Working
{
    public enum EmotionalState
    {
        Random,
        Happy,
        Sad,
        Annoyed,
        Tired
    }
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
        [field: SerializeField] public EmotionalState emotionalState { get; private set; }
        [SerializeField] GameObject[] Eyes;
        public bool interacting { get; private set; }
        [field: SerializeField, Expandable] public OfficeTask task { get; private set; }
        [SerializeField] int startingChanceOfTask;
        float ChanceOfTask { get { return startingChanceOfTask - Time.timeSinceLevelLoad / 50; } }
        [SerializeField] GameObject taskMarker;

        [HideInInspector] public Transform officeStation;
        [HideInInspector] public ActivityRoom breakRoom;
        [HideInInspector] public ActivityRoom[] meetingRooms;

        public event Action<OfficeWorker> EndedActivity;

        WorkInteractable _workInteractable;

        [SerializeField] OfficeAudioManager officeAudio;
        protected override void Awake()
        {
            base.Awake();
            _workInteractable = GetComponent<WorkInteractable>();
            officeAudio = FindFirstObjectByType<OfficeAudioManager>();
            _workInteractable.enabled = false;
        }

        protected virtual async void Start()
        {
            if(emotionalState == EmotionalState.Random)
                SetEmotionalState((EmotionalState)UnityEngine.Random.Range(1, 5));

            while (Application.isPlaying)
            {
                await IsInteracting();

                int activity = UnityEngine.Random.Range(0, 101);

                if(task == null)
                {
                    print(Mathf.RoundToInt(ChanceOfTask));
                    int chance = Mathf.RoundToInt(ChanceOfTask) > 3 ? Mathf.RoundToInt(ChanceOfTask) : 3;
                    int task = UnityEngine.Random.Range(0, chance);
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
            ToggleInteractable();
        }
        public async void StartOfficeTask(Transform interactor)
        {
            if(task == null)
            {
                return;
            }

            if (task.initialized)
            {
                return;
            }

            switch (task.taskType)
            {
                case TaskType.Fetch:
                    officeAudio.PlayVoiceLine(officeAudio.RandomFetchLine());
                    break;
                case TaskType.Cleaning:
                    officeAudio.PlayVoiceLine(officeAudio.RandomCleaningLine());
                    break;
            }

            interacting = true;
            SetDestination(transform);
            transform.LookAt(interactor);
            task.InitializeTask(this);
            taskMarker.SetActive(false);
            task.Completed += ToggleInteractable;

            await Task.Delay(5000);

            SetActivity(Activity.Working);
            interacting = false;
        }
        #endregion

        public void ToggleInteractable()
        {
            _workInteractable.enabled = !_workInteractable.enabled;
        }

        public void SetEmotionalState(EmotionalState state)
        {
            emotionalState = state;

            foreach (var item in Eyes)
            {
                item.SetActive(false);
            }

            Eyes[(int)emotionalState - 1].SetActive(true);
        }
    }
}