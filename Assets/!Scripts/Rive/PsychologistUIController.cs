using System;
using DialogueSystem.Enumerations;
using DialogueSystem.Enumerations.StoryEnum;
using DialogueSystem.Scripts.ScriptableObjects;
using FMOD.Studio;
using FMODUnity;
using Rive;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace _Scripts.Rive
{
    public class PsychologistUIController : MonoBehaviour
    {
        RiveScreen _riveScreen;
        EventInstance _dialogueInstance;
        InputAction _playNextDialogueAction;
        DSDialogueSO _currentDialogue;
        int _sceneToLoad;

        [SerializeField] bool dissolveBackgroundOnStart;
    
        public bool IsActive { get; private set; }
        bool _isMultipleChoice;

        void Start()
        {
            _riveScreen = GetComponent<RiveScreen>();
            _riveScreen.OnRiveEvent += RiveEventHappens;

            _playNextDialogueAction = InputSystem.actions.FindAction("Interact");
        }
        void Update()
        {
            if (_playNextDialogueAction.WasPressedThisFrame() && IsActive && !_isMultipleChoice)
            {
                // Plays disappearing animation, when done RiveEventHappens and plays PlayPsychologistLine
                _riveScreen.StateMachine.GetTrigger("Disappear").Fire();
            }
        }

        void RiveEventHappens(ReportedEvent reportedEvent)
        {
            // Dialogue has disappeared and is ready to play the next dialogue
            if (reportedEvent.Name == "Dialogue Gone Event")
            {
                PlayPsychologistLine();
            }

            if (reportedEvent.Name == "Background Visible Event")
            {
                SceneManager.LoadScene(_sceneToLoad);
            }
            if (_isMultipleChoice)
            {
                if (reportedEvent.Name == "Option 1 Pressed Event")
                {
                    DSDialogueSO nextDialogue = _currentDialogue.choices[0].nextDialogue;
                    _currentDialogue = nextDialogue;
                    _riveScreen.StateMachine.GetTrigger("Disappear").Fire();
                    _isMultipleChoice = false;
                }

                if (reportedEvent.Name == "Option 2 Pressed Event")
                {
                    DSDialogueSO nextDialogue = _currentDialogue.choices[1].nextDialogue;
                    _currentDialogue = nextDialogue;
                    _riveScreen.StateMachine.GetTrigger("Disappear").Fire();
                    _isMultipleChoice = false;
                }
            }
        
        }

        public void SetDialogue(DSDialogueSO dialogue, int sceneIndex)
        {
            _currentDialogue = dialogue;
            _sceneToLoad = sceneIndex;
        
            if (dissolveBackgroundOnStart)
            {
                _riveScreen.StateMachine.GetTrigger("BackgroundRemove").Fire();
            }
            PlayPsychologistLine();
        
        }
        void PlayPsychologistLine()
        {
            if (_currentDialogue)
            {
                IsActive = true;
                switch (_currentDialogue.dialogueType)
                {
                    // For single dialogue
                    case DSDialogueType.SingleChoice:
                    {
                        _riveScreen.SetTextRunAtPath(_currentDialogue.text, RiveScreen.TextPath.Psychologist);

                        NarratorEnumSO narratorEnum = FindAnyObjectByType<NarratorEnumSO>();
                        if (narratorEnum is not null)
                        {
                            switch (narratorEnum.narratorType)
                            {
                                case NarratorEnumSO.NarratorType.Psychologist:
                                    _riveScreen.StateMachine.GetTrigger("PsychologistAppear").Fire();
                                    break;
                                case NarratorEnumSO.NarratorType.Player:
                                    _riveScreen.StateMachine.GetTrigger("PlayerAppear").Fire();
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            _riveScreen.StateMachine.GetTrigger("PsychologistAppear").Fire();
                        }
                   
                        var voiceActing = _currentDialogue.voiceEvent;
                        if (!voiceActing.IsNull)
                        {
                            SetFMODEventAndPlay(voiceActing);
                        }
                        else
                        {
                            Debug.LogError("Dialogue Event is Empty on The current line");
                        }
                        // Stores Next Dialogue
                        DSDialogueSO nextDialogue = _currentDialogue.choices[0].nextDialogue;
                        _currentDialogue = nextDialogue;
                        _isMultipleChoice = false;
                    
                        break;
                    }
                    case DSDialogueType.MultipleChoice:
                
                        // Sets the different Dialogues
                        SetText(RiveScreen.TextPath.Psychologist, _currentDialogue.text);
                        SetText(RiveScreen.TextPath.Option1, _currentDialogue.choices[0].text);
                        SetText(RiveScreen.TextPath.Option2, _currentDialogue.choices[1].text);

                        // For dialogue with options
                        _riveScreen.StateMachine.GetTrigger("Appear").Fire();
                        _isMultipleChoice = true;
                    
                        break;
                }
            }
            else
            {
                if (dissolveBackgroundOnStart)
                    _riveScreen.StateMachine.GetTrigger("BackgroundAdd").Fire();
                else
                    SceneManager.LoadScene(_sceneToLoad);
            }
        }


        void SetText(RiveScreen.TextPath path, string textRun)
        {
            _riveScreen.SetTextRunAtPath(textRun, path);
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

    }
}
