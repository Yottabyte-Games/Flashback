using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Psychiatrist
{
    public class PsychiatristStoryManager : MonoBehaviour
    {
        [SerializeField] DialogueStarter[] dialogues;

        async void Start()
        {
            foreach (var dialogue in dialogues)
            {
                dialogue.StartDialogue();

                //await Task.Delay(dialogue.startingDialogue.voiceEvent.)
            }
        }
    }
}
