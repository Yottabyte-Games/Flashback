using DialogueSystem.Scripts;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;
using Eflatun.SceneReference;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] DSDialogueSO startingDialogue;
    [Header("Only needed if we are swapping scene")]
    [SerializeField]
    SceneReference sceneToLoad;
    [SerializeField] bool loadSceneAfterDialogue = false;

    bool hasTalkedAlready = false;
    DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = GameObject.FindWithTag("MainCamera").GetComponent<DialogueManager>();
        if (dialogueManager is null)
        {
            Debug.LogError("Dialogue Manager is null");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTalkedAlready)
            StartDialogue();
    }

    public void StartDialogue()
    {
        if (loadSceneAfterDialogue)
        {
            dialogueManager.SetDialogue(startingDialogue, sceneToLoad);
            hasTalkedAlready = true;
        }
        else
            dialogueManager.SetDialogue(startingDialogue);
        hasTalkedAlready = true;
    }
}
