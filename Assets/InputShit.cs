using DialogueSystem.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputShit : MonoBehaviour
{
    [SerializeField] ActivateDialouge activeDialogue;
    
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            activeDialogue.NextDialogue();
        }
    }
}
