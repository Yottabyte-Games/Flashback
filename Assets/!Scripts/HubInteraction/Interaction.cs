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
        // Send en stråle fra midten av skjermen for å sjekke om vi ser på noe interaktivt
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionRange))
        {
            DialogueStarter interactable = hit.collider.GetComponent<DialogueStarter>();
            Debug.Log(interactable);
            if (interactable != null && interactionObject == null)
            {
                StartInteraction(interactable);
            }
            else if (interactable == null && interactionObject != null)
            {
                StopInteraction();
            }

            if (interactable != null && Input.GetKeyDown(KeyCode.F)) { interactable.StartDialogue(); }
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