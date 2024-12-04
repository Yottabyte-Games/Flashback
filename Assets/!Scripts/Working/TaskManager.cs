using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Working.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Working
{
    public class TaskManager : MonoBehaviour
    {
        public int tasksCompleted;
        public UnityEvent<int> TaskCompleted;
        public UnityEvent<int> TaskAdded;
        [Expandable, ReadOnly] public List<OfficeTask> currentTasks = new List<OfficeTask>();
    

        [SerializeField] GameObject[] fetchableItems;
        [SerializeField] GameObject[] trash;

        public void AddOfficeTask(OfficeTask task)
        {
            currentTasks.Add(task);
            TaskAdded.Invoke(currentTasks.Count);
        }
        public void CompleteOfficeTask(OfficeTask task)
        {
            tasksCompleted++;
            print(tasksCompleted);
            TaskCompleted.Invoke(tasksCompleted);
            currentTasks.Remove(task);
        }

        public GameObject GenerateTaskItem(TaskType taskType)
        {
            GameObject current = null;
            switch (taskType)
            {
                case TaskType.Fetch:
                    current = Instantiate(fetchableItems[Random.Range(0, fetchableItems.Length)]);
                    break;
                case TaskType.Cleaning:
                    current = Instantiate(trash[Random.Range(0, trash.Length)]);
                    break;
            }

            current.transform.position = itemPositions[Random.Range(0, itemPositions.Count)].transform.position;

            return current;
        }

        [field: SerializeField] public List<Trashcan> trashcans { get; private set; } = new List<Trashcan>();
        [Button]
        async void FindAllTrashcans()
        {
            trashcans.Clear();

            Trashcan[] cans = FindObjectsByType<Trashcan>(FindObjectsSortMode.None);

            for (int i = 0; i < cans.Length; i++)
            {
                trashcans.Add(cans[i]);

                await Task.Delay(10);
            }
        }

        [SerializeField, ReadOnly] List<ItemPosition> itemPositions = new List<ItemPosition>();
        [Button]
        async void FindAllItemPositions()
        {
            itemPositions.Clear();

            ItemPosition[] positions = FindObjectsByType<ItemPosition>(FindObjectsSortMode.None);

            for (int i = 0; i < positions.Length; i++)
            {
                itemPositions.Add(positions[i]);

                await Task.Delay(10);
            }
        }
    }
}
