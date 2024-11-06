using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CleaningTask : OfficeTask
{
    TaskItem toClean;
    List<TaskGoal> goal = new List<TaskGoal>();

    protected override void OnEnable()
    {
        taskType = TaskType.Cleaning;
        base.OnEnable();
    }
    public override void InitializeTask(OfficeWorker worker)
    {
        base.InitializeTask(worker);
        toClean = manager.GenerateTaskItem(taskType).GetComponent<TaskItem>();
        toClean.InteractedWith += ProgressTask;
    }

    protected override void ProgressTask()
    {
        foreach (var can in manager.trashcans)
        {
            TaskGoal current = can.AddComponent<TaskGoal>();
            goal.Add(current);
            current.reached += CompleteTask;
        }
    }
}
