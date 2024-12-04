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
            taskName = "Fetch " + _toFetch.name;
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
