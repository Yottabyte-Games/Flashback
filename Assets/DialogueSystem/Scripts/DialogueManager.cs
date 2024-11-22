using DialogueSystem.Scripts.ScriptableObjects;
using Eflatun.SceneReference;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem.Scripts
{
    [RequireComponent(typeof(GameHudController))]
    public class DialogueManager : MonoBehaviour
    {
        
        GameHudController gameHudController;

        public bool _isDialogueActive { get; private set; }

        DSDialogueSO _currentDialogue;
        InputAction _nextDialogueAction;
        EventInstance _dialogueInstance;
        SceneReference _sceneToLoad;

        void OnValidate()
        {
            if(gameHudController == null)
                gameHudController = GetComponent<GameHudController>();
        }

        void Start()
        {
            if (gameHudController == null)
                gameHudController = GetComponent<GameHudController>();

            _nextDialogueAction = InputSystem.actions.FindAction("Interact");
        }

        void Update()
        {

            if (_nextDialogueAction.WasPressedThisFrame() && _isDialogueActive)
            {
                PlayDialogueLine();
            }
        }

        public void SetDialogue(DSDialogueSO startingDialogue, SceneReference scene = null)
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

                var voiceActing = _currentDialogue.voiceEvent;
                // Stores Next Dialogue
                DSDialogueSO nextDialogue = _currentDialogue.choices[0].nextDialogue;
                _currentDialogue = nextDialogue;

                if (!voiceActing.IsNull)
                {
                    SetFMODEventAndPlay(voiceActing);
                }
                else
                {
                    Debug.LogError("Dialogue Event is Empty on The current line");
                }
                
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

        void SetFMODEventAndPlay(EventReference voiceEvent)
        {
            // Stop the currently playing instance if it exists
            if (_dialogueInstance.isValid())
            {
                _dialogueInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                _dialogueInstance.release();
            }

            // Create and start the new instance
            _dialogueInstance = RuntimeManager.CreateInstance(voiceEvent);
            _dialogueInstance.start();
        }

        public void StopDialog()
        {
            _dialogueInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}