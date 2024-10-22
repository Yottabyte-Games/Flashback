using Unity.Jobs;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Camera camera;
    public float range = 100f;
    Interactable interactable;
    GameObject interactableObject = null;

    private void Start()
    {
;
    }

    void Update()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Interactable hitInteractable = hit.collider.tag == "Interactable" ? hit.collider.GetComponent<Interactable>() : null;

            // Outline Enable/Disable
            if (hitInteractable != null)
            {
                EnableInteraction(hitInteractable);
            }
            else
            {
                DisableInteraction();
            }

            // Item interaction
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (interactableObject == null && hitInteractable != null)
                {
                    hitInteractable.Interact();
                    if (hitInteractable.canInteract)
                    {
                        interactableObject = hitInteractable.gameObject;
                        Debug.Log(interactableObject);
                    }
                }
                else if (interactableObject != null)
                {
                    PlaceObject(hit);
                }
            }
        }
        else
        {
            DisableInteraction();
        }
    }

    void PlaceObject(RaycastHit hit)
    {
        interactable.DisableInteraction();

        //Check if snowball is large enough.
        if (hit.transform.localScale.y > 1)
        {
            GameObject placedObject = Instantiate(interactableObject, hit.point, Quaternion.identity);
            placedObject.transform.up = hit.normal;  // Rotate to surface normal.
            placedObject.SetActive(true);
  
        }

        //Else drop on ground
        else 
        {
            GameObject placedObject = Instantiate(interactableObject, hit.point+new Vector3(0,1,0), Quaternion.identity);
        }

        
        interactableObject = null;
    }

    void DisableInteraction()
    {
        if (interactable != null)
        {
            interactable.DisableOutline();
            interactable = null;
        }
    }

    // Enables interaction outline on item
    void EnableInteraction(Interactable newInteractable)
    {
        if (interactable != newInteractable)
        {
            DisableInteraction();
            interactable = newInteractable;
            interactable.EnableOutline();
        }
    }
}
