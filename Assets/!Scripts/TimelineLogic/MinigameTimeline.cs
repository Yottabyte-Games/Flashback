using _Scripts;
using UnityEngine;
using UnityEngine.Playables;

public class MinigameTimeline : MonoBehaviour
{
    PlayableDirector playableDirector;
    
    [SerializeField] private StoryBeat storyBeatToActOn;
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && StoryManager.StoryBeat == storyBeatToActOn)
        {
            playableDirector.Play();
        }
    }
}
