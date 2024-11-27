using UnityEngine;
using UnityEngine.Playables;

public class MinigameTimeline : MonoBehaviour
{
    PlayableDirector playableDirector;
    
    [SerializeField] TimelineState timelineState;
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        
        if (timelineState.hasPlayed)
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            print("collided with " + other.name);
            playableDirector.Play();
            timelineState.hasPlayed = true;
        }
    }
    // TODO: Remove when building
    private void OnDestroy()
    {
        timelineState.hasPlayed = false;
    }
}
