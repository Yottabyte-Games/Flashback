using UnityEngine;

public enum TaskType
{
    Assistance,
    Cleaning, 
    Message,

}

[CreateAssetMenu(fileName = "Task", menuName = "Minigames/Office/Task")]
public class OfficeTask : ScriptableObject
{
    private void OnEnable()
    {
        Debug.Log("new task");
    }
}
