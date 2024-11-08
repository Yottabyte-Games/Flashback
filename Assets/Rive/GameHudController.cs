using Plugins.Rive.UI;
using UnityEngine;

public class GameHudController : MonoBehaviour
{
    [SerializeField] private RiveScreen riveScreen;
    

    public void StartDialogue(string dialogueString)
    {
        SetDialogue(dialogueString);
        riveScreen.stateMachine.GetTrigger("AddDialogue").Fire();
    }

    public void NextDialogue(string dialogueString)
    {
        SetDialogue(dialogueString);
        riveScreen.stateMachine.GetTrigger("NextDialogue").Fire();

    }
    
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
