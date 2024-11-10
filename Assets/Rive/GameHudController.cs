using System;
using Plugins.Rive.UI;
using UnityEngine;

public class GameHudController : MonoBehaviour
{
    [SerializeField] private RiveScreen riveScreen;

    private void Start()
    {
        if (riveScreen == null)
        {
            riveScreen = GetComponent<RiveScreen>();
            if (riveScreen == null)
            {
                Debug.LogError("No RiveScreen component found on " + gameObject.name);
            }
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
    public void EndDialogue()
    {
        riveScreen.stateMachine.GetTrigger("RemoveDialogue").Fire();
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
    private void SetDialogue(string dialogue)
    {
        riveScreen.SetDialogue(dialogue);
    }
}
