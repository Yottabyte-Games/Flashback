using Eflatun.SceneReference;
using DialogueSystem.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DialogueSystem.Scripts
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] GameHudController gameHudController;


        DSDialogueSO currentDialogue;
        bool isDialogueActive;
        SceneReference sceneToLoad;
        
        InputAction NextDialogueAction;

        void Start()
        {
            NextDialogueAction = InputSystem.actions.FindAction("Interact");
        }

        void Update()
        {
            if (NextDialogueAction.WasPressedThisFrame() && isDialogueActive)
            {
                PlayDialogueLine();
            }
        }

        public void SetDialogue(DSDialogueSO startingDialogue, SceneReference scene = null)
        {
            currentDialogue = startingDialogue;
            PlayDialogueLine();
            sceneToLoad = scene;
        }
        
        void PlayDialogueLine()
        {
            print("Playing Dialogue");
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
                if (sceneToLoad!=null)
                {
                    gameHudController.EndDialogue(sceneToLoad);
                }
                gameHudController.EndDialogue();
                isDialogueActive = false;
            }
        }
    }
}