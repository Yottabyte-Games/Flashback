using DialogueSystem.Scripts;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;

public class InputShit : MonoBehaviour
{
    [SerializeField] private DSDialogueSO startingDialogue;
    
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = GameObject.FindWithTag("MainCamera").GetComponent<DialogueManager>();
        if (dialogueManager == null)
        {
            print("Dialogue Manager is null");
        }
    }

    public void StartDialogue()
    {
        dialogueManager.SetDialogue(startingDialogue);
    }
}
