using UnityEngine;

namespace _Scripts.Snowman_Scripts.Interaction
{
    public class Interaction : MonoBehaviour
    {
<<<<<<< HEAD
        public Camera camera;
        public float range = 100f;
        Interactable interactable;
        GameObject interactableObject = null;
        GameObject ghostObject = null;

        float mouseZoom = 1;

        public Material ghostMaterial;

        void Start()
        {
            ;
        }

        void Update()
        {
            //mouseZoom += Input.GetAxis("Mouse ScrollWheel");
            mouseZoom = Mathf.Clamp(mouseZoom+ Input.GetAxis("Mouse ScrollWheel")*8, 1f, 20);
            Debug.Log(mouseZoom);

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                Interactable hitInteractable = hit.collider.tag == "Interactable" ? hit.collider.GetComponent<Interactable>() : null;

                // Outline Enable/Disable
                if (hitInteractable != null)
=======
        public Camera mainCamera;
        public float range = 100f;
        Interactable _interactable;
        GameObject _interactableObject = null;
        GameObject _ghostObject = null;

        float _mouseZoom = 1;

        public Material ghostMaterial;

        void Update()
        {
            //mouseZoom += Input.GetAxis("Mouse ScrollWheel");
            _mouseZoom = Mathf.Clamp(_mouseZoom+ Input.GetAxis("Mouse ScrollWheel")*8, 1f, 20);
            Debug.Log(_mouseZoom);

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                Interactable hitInteractable = hit.collider.CompareTag("Interactable")
                    ? hit.collider.GetComponent<Interactable>()
                    : null;

                // Outline Enable/Disable
                if (hitInteractable is not null)
>>>>>>> Build
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
<<<<<<< HEAD
                    if (interactableObject == null && hitInteractable != null)
=======
                    if (_interactableObject is null && hitInteractable is not null)
>>>>>>> Build
                    {
                        hitInteractable.Interact();
                        if (hitInteractable.canInteract)
                        {
<<<<<<< HEAD
                            interactableObject = hitInteractable.gameObject;
                            Debug.Log(interactableObject);
                        }
                    }
                    else if (interactableObject != null)
=======
                            _interactableObject = hitInteractable.gameObject;
                            Debug.Log(_interactableObject);
                        }
                    }
                    else if (_interactableObject is not null)
>>>>>>> Build
                    {
                        PlaceObject(hit);
                    }
                }
            }
            else
            {
                DisableInteraction();
            }

<<<<<<< HEAD
            if(interactableObject != null) { GhostBlock(hit); }
=======
            if(_interactableObject is not null) { GhostBlock(hit); }
>>>>>>> Build
        }


        void GhostBlock(RaycastHit hit)
        {
<<<<<<< HEAD
            if(ghostObject == null && hit.collider.gameObject != null)
            {
                ghostObject = Instantiate(interactableObject, hit.point, Quaternion.identity);
                ghostObject.SetActive(true);

                // "Disables" Rigidbody
                Rigidbody rb = ghostObject.GetComponent<Rigidbody>();
=======
            if(_ghostObject is null && hit.collider.gameObject is not null)
            {
                _ghostObject = Instantiate(_interactableObject, hit.point, Quaternion.identity);
                _ghostObject.SetActive(true);

                // "Disables" Rigidbody
                Rigidbody rb = _ghostObject.GetComponent<Rigidbody>();
>>>>>>> Build
                rb.isKinematic = true;
                rb.detectCollisions = false;
            
                //Sets material to transparent selected material
<<<<<<< HEAD
                ghostObject.GetComponent<MeshRenderer>().material = ghostMaterial;
            }
            else
            {

                //Positions Ghostblock on update
                ghostObject.gameObject.transform.position = hit.point + ghostObject.transform.up/(1f+mouseZoom);
                ghostObject.transform.up = hit.normal;
=======
                _ghostObject.GetComponent<MeshRenderer>().material = ghostMaterial;
            }
            else
            {
                //Positions Ghostblock on update
                _ghostObject.gameObject.transform.position = hit.point + _ghostObject.transform.up/(1f+_mouseZoom);
                _ghostObject.transform.up = hit.normal;
>>>>>>> Build
            }

        }



        void PlaceObject(RaycastHit hit)
        {
<<<<<<< HEAD
            if(hit.collider.tag == "Interactable"){ interactable.DisableInteraction(); }
        

       
            GameObject placedObject = Instantiate(interactableObject, ghostObject.transform.position, Quaternion.identity);
            placedObject.transform.up = hit.normal;  // Rotate to surface normal.
            placedObject.SetActive(true);

            Destroy(ghostObject);
            Debug.Log(ghostObject);
  
        
        
            interactableObject = null;
=======
            if(hit.collider.CompareTag("Interactable")){ _interactable.DisableInteraction(); }
            
            GameObject placedObject = Instantiate(_interactableObject, _ghostObject.transform.position, Quaternion.identity);
            placedObject.transform.up = hit.normal;  // Rotate to surface normal.
            placedObject.SetActive(true);

            Destroy(_ghostObject);
            Debug.Log(_ghostObject);
  
        
        
            _interactableObject = null;
>>>>>>> Build
        }

        //Disables outline on item
        void DisableInteraction()
        {
<<<<<<< HEAD
            if (interactable != null)
            {
                interactable.DisableOutline();
                interactable = null;
            }
=======
            if (_interactable is null) return;
            _interactable.DisableOutline();
            _interactable = null;
>>>>>>> Build
        }

        // Enables interaction outline on item
        void EnableInteraction(Interactable newInteractable)
        {
<<<<<<< HEAD
            if (interactable != newInteractable)
            {
                DisableInteraction();
                interactable = newInteractable;
                interactable.EnableOutline();
            }
=======
            if (_interactable == newInteractable) return;
            DisableInteraction();
            _interactable = newInteractable;
            _interactable.EnableOutline();
>>>>>>> Build
        }
    }
}