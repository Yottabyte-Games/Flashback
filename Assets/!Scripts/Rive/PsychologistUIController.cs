using DialogueSystem.Scripts.ScriptableObjects;
using DS.Enumerations;
using FMOD.Studio;
using FMODUnity;
using Rive;
using UnityEngine;
using UnityEngine.InputSystem;

public class PsychologistUIController : MonoBehaviour
{
    RiveScreen riveScreen;
    EventInstance _dialogueInstance;
    private InputAction _playNextDialogueAction;
    [Header("Start Dialogue")]
    [SerializeField] DSDialogueSO _currentDialogue;
    
    private bool _isMultipleChoice;

    private void Start()
    {
        riveScreen = GetComponent<RiveScreen>();
        riveScreen.OnRiveEvent += RiveEventHappens;

        _playNextDialogueAction = InputSystem.actions.FindAction("Interact");
        
        PlayPsychologistLine();
    }
    void Update()
    {
        if (_playNextDialogueAction.WasPressedThisFrame() && !_isMultipleChoice)
        {
            // Plays disappearing animation, when done RiveEventHappens and plays PlayPsychologistLine
            riveScreen.stateMachine.GetTrigger("Disappear").Fire();
        }
    }

    private void RiveEventHappens(ReportedEvent reportedEvent)
    {
        // Dialogue has disappeared and is ready to play the next dialogue
        if (reportedEvent.Name == "Dialogue Gone Event")
        {
            PlayPsychologistLine();
        }
        if (_isMultipleChoice)
        {
            if (reportedEvent.Name == "Option 1 Pressed Event")
            {
                DSDialogueSO nextDialogue = _currentDialogue.choices[0].nextDialogue;
                _currentDialogue = nextDialogue;
                riveScreen.stateMachine.GetTrigger("Disappear").Fire();
            }

            if (reportedEvent.Name == "Option 2 Pressed Event")
            {
                DSDialogueSO nextDialogue = _currentDialogue.choices[1].nextDialogue;
                _currentDialogue = nextDialogue;
                riveScreen.stateMachine.GetTrigger("Disappear").Fire();
            }
        }
    }

    private void PlayPsychologistLine()
    {
        if (_currentDialogue)
        {
            switch (_currentDialogue.dialogueType)
            {
                // For single dialogue
                case DSDialogueType.SingleChoice:
                {
                    riveScreen.SetTextRunAtPath(_currentDialogue.text, RiveScreen.TextPath.Psychologist);
            
                    riveScreen.stateMachine.GetTrigger("PsychologistAppear").Fire();
                    
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
                    riveScreen.stateMachine.GetTrigger("Appear").Fire();
                    _isMultipleChoice = true;
                    
                    break;
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
        }
    }
    
 
    private void SetText(RiveScreen.TextPath path, string textRun)
    {
        riveScreen.SetTextRunAtPath(textRun, path);
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
