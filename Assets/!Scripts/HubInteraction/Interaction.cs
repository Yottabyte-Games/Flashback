using _Scripts.Rive;
using UnityEngine;
using UnityEngine.InputSystem;
namespace _Scripts.HubInteraction
{
    public class Interaction : MonoBehaviour
    {
        public float interactionRange = 3f;
        public Camera playerCamera;

        GameObject interactionObject;

        GameHudController ghd;

        InputAction _interactAction;

        void Start()
        {
            playerCamera = Camera.main;
            ghd = GetComponent<GameHudController>();
            _interactAction = InputSystem.actions.FindAction("Interact");
        }

        void Update()
        {
            Interact();
        }

        void Interact()
        {
            RaycastHit hit;
            if (UnityEngine.Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
            {
                DialogueStarter interactable = hit.collider.GetComponent<DialogueStarter>();
                if (hit.collider.tag == "Interactable" && interactionObject == null)
                {
                    StartInteraction(interactable);
                }
                else if (hit.collider.tag != "Interactable" && interactionObject != null)
                {
                    StopInteraction();
                }

                if (interactable != null && _interactAction.WasPressedThisFrame())
                {
                    interactable.StartDialogue();
                    interactable.gameObject.GetComponent<StoryProgresser>().SelectNextStoryBeat();
                }
            }
            else if (interactionObject is not null){ StopInteraction();}
        }

        void StartInteraction(DialogueStarter interactable)
        {
            interactionObject = interactable.gameObject;
            ghd.HoverOn(interactable.name);
//        Debug.Log("Started Interaction");
        }

        void StopInteraction()
        {
//        Debug.Log("Stopped Interaction");
            interactionObject = null;
            ghd.HoverOff();
        }

    }
}