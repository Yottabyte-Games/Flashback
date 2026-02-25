using System;
using UnityEngine;

namespace _Scripts.Working.Tasks
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
            
            int startIndex = _toFetch.name.IndexOf("(Clone)", StringComparison.Ordinal);
            _toFetch.name = _toFetch.name.Remove(startIndex).Trim();
            taskName = "Fetch " + _toFetch.name + " for " + worker.name;
            
        }

        protected override void ProgressTask()
        {
            Debug.Log("progress");
            _goal = creator.gameObject.AddComponent<TaskGoal>();
            _goal.Reached += CompleteTask;

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
