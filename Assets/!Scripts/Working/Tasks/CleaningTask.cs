using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Working.Tasks
{
    public class CleaningTask : OfficeTask
    {
        TaskItem _toClean;
        readonly List<TaskGoal> _goal = new List<TaskGoal>();

        protected override void OnEnable()
        {
            taskType = TaskType.Cleaning;
            base.OnEnable();
        }
        public override void InitializeTask(OfficeWorker worker)
        {
            base.InitializeTask(worker);
            _toClean = manager.GenerateTaskItem(taskType).GetComponent<TaskItem>();
            _toClean.InteractedWith += ProgressTask;
            
            int startIndex = _toClean.name.IndexOf("(Clone)", StringComparison.Ordinal);
            _toClean.name = _toClean.name.Remove(startIndex).Trim();
            taskName = "Clean " + _toClean.name;
        }

        protected override void ProgressTask()
        {
            Debug.Log("progress");
            foreach (var can in manager.trashcans)
            {
                TaskGoal current = can.gameObject.AddComponent<TaskGoal>();
                _goal.Add(current);
                current.Reached += CompleteTask;
            }
        }
    }
}
