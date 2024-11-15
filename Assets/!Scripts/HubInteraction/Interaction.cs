using _Scripts.Snowman_Scripts.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    public float interactionRange = 3f;
    public Camera playerCamera;

    private GameObject interactionObject;

    private GameHudController ghd;

    private void Start()
    {
        ghd = GetComponent<GameHudController>();
    }

    void Update()
    {
        Interact();
    }

    void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
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

            if (interactable != null && Input.GetKeyDown(KeyCode.E)) { interactable.StartDialogue(); }
        }
        else if (interactionObject != null){ StopInteraction();}
    }

    void StartInteraction(DialogueStarter interactable)
    {
        interactionObject = interactable.gameObject;
        ghd.HoverOn(interactable.name);
        Debug.Log("Started Interaction");
    }

    void StopInteraction()
    {
        Debug.Log("Stopped Interaction");
        interactionObject = null;
        ghd.HoverOff();
    }

}