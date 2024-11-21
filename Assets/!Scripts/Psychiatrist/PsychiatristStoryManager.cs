using DialogueSystem.Scripts;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Psychiatrist
{
    public class PsychiatristStoryManager : MonoBehaviour
    {
        [SerializeField] DialogueStarter[] dialogues;
        DialogueManager dialogueManager;

        async void Start()
        {
            dialogueManager = FindFirstObjectByType<DialogueManager>();

            foreach (var dialogue in dialogues)
            {
                dialogue.StartDialogue();


                //dialogueManager._currentDialogue.voiceEvent.
                //await Task.Delay(dialogue.startingDialogue.voiceEvent.)
            }
        }
    }
}
