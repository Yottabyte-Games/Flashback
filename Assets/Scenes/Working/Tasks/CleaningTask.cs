using System.Collections.Generic;
<<<<<<< HEAD:Assets/Scenes/Working/Tasks/CleaningTask.cs
using UnityEngine;
=======
using Unity.VisualScripting;
>>>>>>> Build:Assets/!Scripts/Working/Tasks/CleaningTask.cs

namespace _Scripts.Working.Tasks
{
    public class CleaningTask : OfficeTask
    {
        TaskItem _toClean;
        List<TaskGoal> _goal = new List<TaskGoal>();

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
        }

        protected override void ProgressTask()
        {
            foreach (var can in manager.trashcans)
            {
                TaskGoal current = can.AddComponent<TaskGoal>();
                _goal.Add(current);
                current.reached += CompleteTask;
            }
        }
    }
}
