using DialogueSystem.Scripts;
using DialogueSystem.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

public class OfficeAudioManager : MonoBehaviour
{
    public List<DSDialogueSO> FetchLines = new();
    public List<DSDialogueSO> TrashLines = new();

    [SerializeField] DialogueManager dialogueManager;

    public DSDialogueSO RandomFetchLine()
    {
        return FetchLines[Random.Range(0, FetchLines.Count)];
    }
    public DSDialogueSO RandomCleaningLine()
    {
        return FetchLines[Random.Range(0, TrashLines.Count)];
    }

    public void PlayVoiceLine(DSDialogueSO dialogue)
    {
        dialogueManager.SetDialogue(dialogue);
    }
}
