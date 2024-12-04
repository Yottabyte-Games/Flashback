using _Scripts.Rive;
using DialogueSystem.Scripts;
using UnityEngine;
using UnityEngine.Playables;
namespace _Scripts.TimelineLogic
{
    public class TimelinePlayer : MonoBehaviour
    {
        [SerializeField] DialogueManager dialogueManager;
    
        DialogueStarter _dialogueStarter;
        PlayableDirector _director;
        void Awake()
        {
            _dialogueStarter = GetComponent<DialogueStarter>();
            _director = GetComponent<PlayableDirector>();
            _director.played += Director_Played;
            _director.stopped += Director_Stopped;
        }
        void Director_Stopped(PlayableDirector playableDirector)
        {
        
        }
        void Director_Played(PlayableDirector playableDirector)
        {
        
        }
        public void StartNarrationTextAndAudio()
        {
        
            //_director.Play();
        }
    }
}
