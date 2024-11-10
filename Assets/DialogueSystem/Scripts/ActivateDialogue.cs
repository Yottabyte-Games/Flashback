using DialogueSystem.Scripts.Data;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;

namespace DialogueSystem.Scripts
{
    public class ActivateDialouge : MonoBehaviour
    {
        [SerializeField] DSDialogueSO startingDialogue;
        [SerializeField] DSDialogueContainerSO dialogueContainer;
        [SerializeField] GameHudController gameHudController;

        DSDialogueSO currentDialogue;
        
        int dialogueIndex = 0;

        void Awake()
        {
            dialogueIndex = 0;
            currentDialogue = startingDialogue;
        }
        
        public void NextDialogue()
        {
            OnOptionChosen(dialogueIndex);
        }
        void OnOptionChosen(int choiceIndex)
        {
            Debug.Log("Choice Index: " + choiceIndex);
            if (choiceIndex < dialogueContainer.UngroupedDialogues.Count)
            {
                // If first dialogue load Text Box
                if (currentDialogue.IsStartingDialogue)
                {
                    gameHudController.StartDialogue(currentDialogue.Text);
                }
                // For every other text go to next text
                else
                {
                    gameHudController.NextDialogue(currentDialogue.Text);
                }
                //Stores Next Dialogue
                DSDialogueSO nextDialogue = currentDialogue.Choices[0].NextDialogue;
                currentDialogue = nextDialogue;
            }
            else
            {
                gameHudController.EndDialogue();
            }
            dialogueIndex++;
        }
    }
}