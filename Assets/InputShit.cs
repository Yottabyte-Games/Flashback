using System;
using DialogueSystem.Scripts;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;
using Eflatun.SceneReference;

public class InputShit : MonoBehaviour
{
    [SerializeField] private DSDialogueSO startingDialogue;
    [Header("Only needed if we are swapping scene")]
    [SerializeField] private SceneReference sceneToLoad;
    
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = GameObject.FindWithTag("MainCamera").GetComponent<DialogueManager>();
        if (dialogueManager == null)
        {
            print("Dialogue Manager is null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StartDialogue();
    }

    private void StartDialogue()
    {
        dialogueManager.SetDialogue(startingDialogue);
    }
}
