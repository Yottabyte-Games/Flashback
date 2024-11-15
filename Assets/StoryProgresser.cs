using UnityEngine;
using UnityEngine.Events;

public class StoryProgresser : MonoBehaviour
{
    public UnityEvent OnProgressedStory;

    private void OnTriggerEnter(Collider other)
    {
        StoryManager.ProgressStory();
        OnProgressedStory?.Invoke();
    }
}
