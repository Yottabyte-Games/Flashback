using Eflatun.SceneReference;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;
using FMOD.Studio;

namespace DialogueSystem.Scripts
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] GameHudController gameHudController;
        [SerializeField] EventReference dialogueEvent;

        bool _isDialogueActive;

        DSDialogueSo _currentDialogue;
        InputAction _nextDialogueAction;
        EventInstance _dialogueInstance;
        SceneReference _sceneToLoad;

        void Start()
        {
            _nextDialogueAction = InputSystem.actions.FindAction("Interact");
            _dialogueInstance = RuntimeManager.CreateInstance(dialogueEvent);

        }

        void Update()
        {
            if (_nextDialogueAction.WasPressedThisFrame() && _isDialogueActive)
            {
                PlayDialogueLine();
            }
        }

        public void SetDialogue(DSDialogueSo startingDialogue, SceneReference scene = null)
        {
            _currentDialogue = startingDialogue;
            PlayDialogueLine();
            _sceneToLoad = scene;
        }
        
        void PlayDialogueLine()
        {
            print("Playing Dialogue");
            if (_currentDialogue is not null)
            {
                // If first dialogue load Text Box
                if (_currentDialogue.isStartingDialogue)
                {
                    gameHudController.StartDialogue(_currentDialogue.text);
                    _isDialogueActive = true;
                }
                // For every other text go to next text
                else
                {
                    gameHudController.NextDialogue(_currentDialogue.text);
                }
                
                SetFMODParameterAndPlay(_currentDialogue.voiceClipIndex);

                
                //Stores Next Dialogue
                DSDialogueSo nextDialogue = _currentDialogue.choices[0].nextDialogue;
                _currentDialogue = nextDialogue;
            }
            else
            {
                if (_sceneToLoad is not null)
                {
                    gameHudController.EndDialogue(_sceneToLoad);
                }

                gameHudController.EndDialogue();
                _isDialogueActive = false;
            }
        }
        void SetFMODParameterAndPlay(int voiceClipIndex)
        {
            _dialogueInstance.setParameterByName("VoiceClipIndex", voiceClipIndex);
            _dialogueInstance.start();
        }
    }
}