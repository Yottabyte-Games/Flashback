<<<<<<< HEAD:Assets/Scenes/Working/Tasks/FetchTask.cs
using NaughtyAttributes;
using UnityEngine;

public class FetchTask : OfficeTask
=======
namespace _Scripts.Working.Tasks
>>>>>>> Build:Assets/!Scripts/Working/Tasks/FetchTask.cs
{
    public class FetchTask : OfficeTask
    {
        TaskItem _toFetch;
        TaskGoal _goal;

        protected override void OnEnable()
        {
            taskType = TaskType.Fetch;
            base.OnEnable();
        }

        public override void InitializeTask(OfficeWorker worker)
        {
            base.InitializeTask(worker);

            _toFetch = manager.GenerateTaskItem(taskType).GetComponent<TaskItem>();
            _toFetch.InteractedWith += ProgressTask;
        }

        protected override void ProgressTask()
        {
            _goal = creator.gameObject.AddComponent<TaskGoal>();
            _goal.reached += CompleteTask;

            Line line = _toFetch.gameObject.GetComponent<Line>();
            line.enabled = true;
            line.stringPoints.Add(_toFetch.transform);
            line.stringPoints.Add(_goal.transform);
        }

        public override void CompleteTask()
        {
            Destroy(_toFetch.gameObject);
            base.CompleteTask();
        }
    }
}
