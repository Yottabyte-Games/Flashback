using UnityEngine;
using UnityEngine.Playables;

public class TimelinePlayer : MonoBehaviour
{
    //public GameObject ControlPanel;
    
    PlayableDirector _director;
    void Awake()
    {
        _director = GetComponent<PlayableDirector>();
        _director.played += Director_Played;
        _director.stopped += Director_Stopped;
    }
    void Director_Stopped(PlayableDirector playableDirector)
    {
        //ControlPanel.SetActive(true);
    }
    void Director_Played(PlayableDirector playableDirector)
    {
        //ControlPanel.SetActive(false);
    }
    public void StartTimeline()
    {
        _director.Play();
    }
}
