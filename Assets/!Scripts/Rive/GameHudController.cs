using System;
using Eflatun.SceneReference;
using Plugins.Rive.UI;
using Rive;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHudController : MonoBehaviour
{
    [SerializeField] RiveScreen riveScreen;
    SceneReference _sceneToLoad;

    void Start()
    {
        if (riveScreen == null)
        {
            riveScreen = GetComponent<RiveScreen>();
            if (riveScreen == null)
            {
                Debug.LogError("No RiveScreen component found on " + gameObject.name);
            }
        }
       
        riveScreen = GetComponent<RiveScreen>();

        riveScreen.OnRiveEvent += RiveEventHandler;
    }

    private void RiveEventHandler(ReportedEvent reportedevent)
    {
        if (reportedevent.Name == "FlashbackEvent" && _sceneToLoad != null)
        {
            SceneManager.LoadScene(_sceneToLoad.Name);
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
    public void EndDialogue(SceneReference sceneReference)
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
