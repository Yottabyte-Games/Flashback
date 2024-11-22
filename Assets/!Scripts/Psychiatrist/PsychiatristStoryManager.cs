using DialogueSystem.Scripts;
using DialogueSystem.Scripts.ScriptableObjects;
using NaughtyAttributes;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace _Scripts.Psychiatrist
{
    public class PsychiatristStoryManager : MonoBehaviour
    {
        [SerializeField] Dialogue[] dialogues;
        DialogueManager dialogueManager;

        async void Start()
        {
            dialogueManager = GetComponent<DialogueManager>();

            await Task.Delay(2000);

            foreach (var dialogue in dialogues)
            {
                StartDualogue(dialogue.dialogue);

                while(dialogueManager._isDialogueActive)
                {
                    await Task.Delay(100);
                }

                if(dialogue.ChangeSceneTo > 0)
                {
                    SceneManager.LoadScene(dialogue.ChangeSceneTo);
                }
            }
        }

        void StartDualogue(DSDialogueSO dialogue)
        {
            dialogueManager.SetDialogue(dialogue);
        }
    }

    [Serializable]
    public class Dialogue
    {
        public DSDialogueSO dialogue;
        [Scene, InfoBox("0 = Don't change scene")]
        public int ChangeSceneTo;
    }
}
