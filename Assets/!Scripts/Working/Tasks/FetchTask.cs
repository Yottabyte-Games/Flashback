using NaughtyAttributes;
using UnityEngine;

public class FetchTask : OfficeTask
{
    TaskItem toFetch;
    TaskGoal goal;

    protected override void OnEnable()
    {
        taskType = TaskType.Fetch;
        base.OnEnable();
    }

    public override void InitializeTask()
    {
        toFetch = manager.GenerateTaskItem(taskType).GetComponent<TaskItem>();
        toFetch.InteractedWith += ProgressTask;

        base.InitializeTask();
    }
}
