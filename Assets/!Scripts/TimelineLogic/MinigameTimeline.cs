using System;
using UnityEngine;
using UnityEngine.Playables;

public class MinigameTimeline : MonoBehaviour
{
    PlayableDirector playableDirector;
    
    [SerializeField] TimelineState timelineState;
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.enabled = false;

        if (timelineState.hasPlayed)
        {
            GetComponent<Collider>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playableDirector.Play();
            playableDirector.enabled = true;
            timelineState.hasPlayed = true;
        }
    }
}
