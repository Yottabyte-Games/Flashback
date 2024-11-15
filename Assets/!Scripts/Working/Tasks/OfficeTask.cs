using System;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Working.Tasks
{
    public enum TaskType
    {
        Assistance,
        Cleaning, 
        Message,
        Fetch
    }

    [CreateAssetMenu(fileName = "Task", menuName = "Minigames/Office/Task")]
    public abstract class OfficeTask : ScriptableObject
    {
        [ReadOnly, SerializeField] protected TaskType taskType;
        protected TaskManager manager { get; private set; }
        protected OfficeWorker creator { get; private set; }

        public bool initialized {  get; private set; }

        public event Action Completed;

        protected virtual void OnEnable()
        {
            manager = FindFirstObjectByType<TaskManager>();
            manager.AddOfficeTask(this);
        }

        [Button]
        public virtual void InitializeTask(OfficeWorker worker)
        {
            creator = worker;
            initialized = true;
        }

        protected abstract void ProgressTask();

        [Button]
        public virtual void CompleteTask()
        {
            Debug.Log("complete");
            Completed?.Invoke();
            manager.CompleteOfficeTask(this);
        }
    }
}