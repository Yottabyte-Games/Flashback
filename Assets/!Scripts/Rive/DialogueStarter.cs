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

    bool hasTalkedAlready = false;
    DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = GameObject.FindWithTag("MainCamera").GetComponent<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue Manager is null");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTalkedAlready)
            StartDialogue();
    }

    void StartDialogue()
    {
        dialogueManager.SetDialogue(startingDialogue, sceneToLoad);
        hasTalkedAlready = true;
    }
}
