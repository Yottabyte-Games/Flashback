using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;


namespace DialogueSystem.Scripts
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameHudController gameHudController;

        private DSDialogueSO currentDialogue;
        private bool isDialogueActive;
        

        public void SetDialogue(DSDialogueSO startingDialogue)
        {
            currentDialogue = startingDialogue;
            PlayDialogueLine();
        }

        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed && isDialogueActive)
            {
                PlayDialogueLine();
            }
        }
        
        void PlayDialogueLine()
        {
            if (currentDialogue != null)
            {
                // If first dialogue load Text Box
                if (currentDialogue.IsStartingDialogue)
                {
                    gameHudController.StartDialogue(currentDialogue.Text);
                    isDialogueActive = true;
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
                isDialogueActive = false;
            }
        }
    }
}