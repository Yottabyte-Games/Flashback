using NaughtyAttributes;
using Unity.VisualScripting;
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

    public override void InitializeTask(OfficeWorker worker)
    {
        base.InitializeTask(worker);

        toFetch = manager.GenerateTaskItem(taskType).GetComponent<TaskItem>();
        toFetch.InteractedWith += ProgressTask;
        goal = creator.gameObject.AddComponent<TaskGoal>();
    }

    protected override void ProgressTask()
    {
        Line line = toFetch.gameObject.GetComponent<Line>();
        line.enabled = true;
        line.stringPoints.Add(toFetch.transform);
        line.stringPoints.Add(goal.transform);
    }
}
