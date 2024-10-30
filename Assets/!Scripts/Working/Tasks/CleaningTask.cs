using UnityEngine;

public class CleaningTask : OfficeTask
{
    TaskItem toClean;
    TaskGoal goal;

    protected override void OnEnable()
    {
        taskType = TaskType.Cleaning;
        base.OnEnable();
    }
    public override void InitializeTask(OfficeWorker worker)
    {
        toClean = manager.GenerateTaskItem(taskType).GetComponent<TaskItem>();
        base.InitializeTask(worker);
    }

    protected override void ProgressTask()
    {
        throw new System.NotImplementedException();
    }
}
