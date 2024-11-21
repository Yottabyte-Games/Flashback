using System;
using Eflatun.SceneReference;
using Plugins.Rive.UI;
using Rive;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RiveScreen))]
public class GameHudController : MonoBehaviour
{
    private RiveScreen riveScreen;
    private SceneReference _sceneToLoad;
    
    private InputAction _pauseAction;


    void Start()
    {
        if (riveScreen is null)
        {
            riveScreen = GetComponent<RiveScreen>();
            if (riveScreen is null)
            {
                Debug.LogError("No RiveScreen component found on " + gameObject.name);
            }
        }

        riveScreen = GetComponent<RiveScreen>();

        riveScreen.OnRiveEvent += RiveEventHandler;
        
        _pauseAction = InputSystem.actions.FindAction("Pause");
    }

    private void RiveEventHandler(ReportedEvent reportedEvent)
    {
        if (reportedEvent.Name == "FlashbackEvent" && _sceneToLoad != null)
        {
            SceneManager.LoadScene(_sceneToLoad.Name);
        }
    }

    private void Update()
    {
        if (_pauseAction.WasPressedThisFrame())
        { 
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            // Set Pause Scene from Rive
            riveScreen.SetRiveScene(RiveScreen.RiveScenes.PauseMenu);
        }
    }


    // First Dialogue should call this
    public void StartDialogue(string dialogueString)
    {
        SetDialogue(dialogueString);
        riveScreen.stateMachine.GetTrigger("AddDialogue").Fire();
    }

    // Every other dialogues calls this
    public void NextDialogue(string dialogueString)
    {
        SetDialogue(dialogueString);
        riveScreen.stateMachine.GetTrigger("NextDialogue").Fire();
    }

    // When last dialogue finishes, call this
    public void EndDialogue(SceneReference sceneReference = null)
    {
        riveScreen.stateMachine.GetTrigger("RemoveDialogue").Fire();
        if (sceneReference != null)
        {
            riveScreen.stateMachine.GetTrigger("FlashBack").Fire();
            _sceneToLoad = sceneReference;
        }
    }

    public void HoverOn(string objectName)
    {
        riveScreen.SetHoverItemName(objectName);
        riveScreen.stateMachine.GetBool("IsHovering").Value = true;
    }

    public void HoverOff()
    {
        riveScreen.stateMachine.GetBool("IsHovering").Value = false;
    }


    // Set Dialogue Text for the next dialogue
    void SetDialogue(string dialogue)
    {
        riveScreen.SetDialogue(dialogue);
    }
}