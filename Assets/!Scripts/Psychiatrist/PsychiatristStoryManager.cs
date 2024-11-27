using DialogueSystem.Scripts.ScriptableObjects;
using NaughtyAttributes;
using System;
using System.Threading.Tasks;
using _Scripts.Rive;
using UnityEngine;


namespace _Scripts.Psychiatrist
{
    public class PsychiatristStoryManager : MonoBehaviour
    {
        [SerializeField] Dialogue dialogue;
        PsychologistUIController _psychologistUIController;
        


        async void Start()
        {
            _psychologistUIController = GetComponent<PsychologistUIController>();

            
            
            await Task.Delay(2000);
            
            StartDialogue(dialogue.dialogue, dialogue.ChangeSceneTo);

            while (_psychologistUIController.isActive)
            {
                await Task.Delay(100);
            }
/*
            if (dialogue.ChangeSceneTo > 0)
            {
                SceneManager.LoadScene(dialogue.ChangeSceneTo);
            }*/
        }

        void StartDialogue(DSDialogueSO dialogue, int sceneIndex)
        {
            _psychologistUIController.SetDialogue(dialogue, sceneIndex);
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
