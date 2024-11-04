using UnityEngine;

namespace _Scripts.Snowman_Scripts.Interaction
{
<<<<<<< HEAD
    public Camera camera;
    public float range = 100f;
    Interactable interactable;
    GameObject interactableObject = null;
    GameObject ghostObject = null;

    private float mouseZoom = 1;

    public Material ghostMaterial;

    private void Start()
=======
    public class Interaction : MonoBehaviour
>>>>>>> main
    {
        public Camera camera;
        public float range = 100f;
        Interactable interactable;
        GameObject interactableObject;

<<<<<<< HEAD
    void Update()
    {
        //mouseZoom += Input.GetAxis("Mouse ScrollWheel");
        mouseZoom = Mathf.Clamp(mouseZoom+ Input.GetAxis("Mouse ScrollWheel")*8, 1f, 20);
        Debug.Log(mouseZoom);

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, range))
=======
        void Update()
>>>>>>> main
        {
            var ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, range))
            {
                var hitInteractable = hit.collider.tag == "Interactable" ? hit.collider.GetComponent<Interactable>() : null;

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
<<<<<<< HEAD
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

        if(interactableObject != null) { GhostBlock(hit); }
    }


    void GhostBlock(RaycastHit hit)
    {
        if(ghostObject == null && hit.collider.gameObject != null)
        {
            ghostObject = Instantiate(interactableObject, hit.point, Quaternion.identity);
            ghostObject.SetActive(true);

            // "Disables" Rigidbody
            Rigidbody rb = ghostObject.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.detectCollisions = false;
            
            //Sets material to transparent selected material
            ghostObject.GetComponent<MeshRenderer>().material = ghostMaterial;
        }
        else
        {

            //Positions Ghostblock on update
            ghostObject.gameObject.transform.position = hit.point + ghostObject.transform.up/(1f+mouseZoom);
            ghostObject.transform.up = hit.normal;
        }

    }



    void PlaceObject(RaycastHit hit)
    {
        if(hit.collider.tag == "Interactable"){ interactable.DisableInteraction(); }
        

       
        GameObject placedObject = Instantiate(interactableObject, ghostObject.transform.position, Quaternion.identity);
        placedObject.transform.up = hit.normal;  // Rotate to surface normal.
        placedObject.SetActive(true);

        Destroy(ghostObject);
        Debug.Log(ghostObject);
  
        
        
        interactableObject = null;
    }

    //Disables outline on item
    void DisableInteraction()
    {
        if (interactable != null)
        {
            interactable.DisableOutline();
            interactable = null;
=======
                var placedObject = Instantiate(interactableObject, hit.point, Quaternion.identity);
                placedObject.transform.up = hit.normal;  // Rotate to surface normal.
                placedObject.SetActive(true);
  
            }

            //Else drop on ground
            else 
            {
                var placedObject = Instantiate(interactableObject, hit.point+new Vector3(0,1,0), Quaternion.identity);
            }

        
            interactableObject = null;
>>>>>>> main
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
}
