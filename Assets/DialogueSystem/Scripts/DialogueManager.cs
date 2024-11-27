using _Scripts.Rive;
using DialogueSystem.Scripts.ScriptableObjects;
using Eflatun.SceneReference;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DialogueSystem.Scripts
{
    [RequireComponent(typeof(GameHudController))]
    public class DialogueManager : MonoBehaviour
    {
        
        GameHudController gameHudController;


        public bool _isDialogueActive { get; private set; }
        [SerializeField] bool canSkipDialogue = true;

        DSDialogueSO _currentDialogue;
        InputAction _nextDialogueAction;
        EventInstance _dialogueInstance;
        SceneReference _sceneToLoad;

        float cd;
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

            if (cd>0)
            {
                cd -= Time.deltaTime;
            }
            else if (_nextDialogueAction.WasPressedThisFrame() && _isDialogueActive && canSkipDialogue)
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
        public void NextDialogue()
        {
            PlayDialogueLine();
        }
        void PlayDialogueLine()
        {
//            print("Playing Dialogue");
            if (_currentDialogue)
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
                StopDialog();
                _isDialogueActive = false;
            }

            cd = .5f;
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

        void StopDialog()
        {
            _dialogueInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }
}