using NaughtyAttributes;
using UnityEngine;

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
    public void CompleteTask()
    {
        manager.CompleteOfficeTask(this);
    }
}
