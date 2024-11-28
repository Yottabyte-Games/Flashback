using System.Collections.Generic;
using _Scripts.Rive;
using UnityEngine;
namespace _Scripts.Fishing
{
    public class DialoguePlayer : MonoBehaviour
    {
        [SerializeField] List<DialogueStarter> dialogueStarters = new();
        [SerializeField] FishingStoryManager manager;
        public void PlayDialogue()
        {
            print(manager.fishCaught);
            dialogueStarters[manager.fishCaught].StartDialogue();
        }
    }
}
