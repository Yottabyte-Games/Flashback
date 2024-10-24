using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "Minigames/Office/Task")]
public class OfficeTask : ScriptableObject
{
    private void OnEnable()
    {
        Debug.Log("new task");
    }
}
