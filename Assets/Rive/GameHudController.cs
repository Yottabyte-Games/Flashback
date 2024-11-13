using Eflatun.SceneReference;
using Plugins.Rive.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHudController : MonoBehaviour
{
    [SerializeField] RiveScreen riveScreen;
    [SerializeField] SceneReference sceneToLoad;

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
            SceneManager.LoadScene(sceneReference.Name);
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
