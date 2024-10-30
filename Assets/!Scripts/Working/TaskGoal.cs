using UnityEngine;

public class TaskGoal : MonoBehaviour
{
    WorkInteractable interactable;
    private void Start()
    {
        interactable = GetComponent<WorkInteractable>();
        interactable.interact.AddListener(Completed);
    }

    public void Completed(Transform transform)
    {

    }
}