using DialogueSystem.Scripts.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace DialogueSystem.Scripts
{
    public class ActivateDialouge : MonoBehaviour
    {
        [SerializeField] DSDialogueSO startingDialogue;
        [SerializeField] TextMeshProUGUI textUI;

        DSDialogueSO currentDialogue;

        void Awake()
        {
            currentDialogue = startingDialogue;
        }

        void ShowText()
        {
            textUI.text = currentDialogue.Text;
        }

        void OnOptionChosen(int choiceIndex)
        {
            DSDialogueSO nextDialogue = currentDialogue.Choices[choiceIndex].NextDialogue;

            if (nextDialogue is null)
            {
                return; // No more dialogues to show, do whatever
            }

            currentDialogue = nextDialogue;

            ShowText();
        }
    }
}
