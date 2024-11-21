using UnityEngine;
using UnityEngine.Events;

public class StoryProgresser : MonoBehaviour
{
    [SerializeField] StoryBeat ActOnBeat;
    [SerializeField] StoryBeat ChangeToBeat;
    public UnityEvent<StoryProgresser> OnProgressedStory;

    private void OnTriggerEnter(Collider other)
    {
        if(StoryManager.StoryBeat != ActOnBeat) return;

        StoryManager.ProgressStory(ChangeToBeat);
        OnProgressedStory?.Invoke(this);
        Destroy(this);
    }
}
