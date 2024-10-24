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
public class OfficeTask : ScriptableObject
{
    TaskManager manager;
    public TaskType taskType;

    private void OnEnable()
    {
        manager = FindFirstObjectByType<TaskManager>();
        RandomizeTask();   
        InitializeTask();
    }

    public void RandomizeTask()
    {
        taskType = (TaskType)Random.Range(0, 5);
    }
    public void InitializeTask()
    {
        switch (taskType)
        {
            case TaskType.Assistance:
                break;
            case TaskType.Cleaning:
                break;
            case TaskType.Message:
                break;
            case TaskType.Fetch:
                InitializeFetchTask();
                break;
        }
    }
    [ShowIf(nameof(taskType), TaskType.Fetch)]
    public GameObject toFetch;
    void InitializeFetchTask()
    {
        toFetch = manager.GenerateFetchableItem();
    }
}
