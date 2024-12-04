namespace _Scripts.Snowman_Scripts.Interaction
{ 
    using UnityEngine;
    public class Interaction : MonoBehaviour
    {
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
            _mouseZoom = Mathf.Clamp(_mouseZoom + Input.GetAxis("Mouse ScrollWheel") * 8, 1f, 20);
//            Debug.Log(_mouseZoom);

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                Interactable hitInteractable = hit.collider.CompareTag("Interactable")
                    ? hit.collider.GetComponent<Interactable>()
                    : null;

                // Outline Enable/Disable
                if (hitInteractable is not null)
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
                    if (_interactableObject is null && hitInteractable is not null)
                    {
                        hitInteractable.Interact();
                        if (hitInteractable.canInteract)
                        {
                            _interactableObject = hitInteractable.gameObject;
//                            Debug.Log(_interactableObject);
                        }
                    }
                    else if (_interactableObject is not null)
                    {
                        PlaceObject(hit);
                    }
                }
            }
            else
            {
                DisableInteraction();
            }

            if (_interactableObject is not null) { GhostBlock(hit); }
        }


        void GhostBlock(RaycastHit hit)
        {
            if (_ghostObject is null && hit.collider.gameObject is not null)
            {
                _ghostObject = Instantiate(_interactableObject, hit.point, Quaternion.identity);
                _ghostObject.SetActive(true);

                // "Disables" Rigidbody
                Rigidbody rb = _ghostObject.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.detectCollisions = false;

                //Sets material to transparent selected material
                _ghostObject.GetComponent<MeshRenderer>().material = ghostMaterial;
            }
            else
            {
                //Positions Ghostblock on update
                Transform _ghostTrans = _ghostObject.transform;

                _ghostTrans.position = hit.point + Vector3.ClampMagnitude(_ghostTrans.up / (1f + _mouseZoom), _ghostTrans.localScale.magnitude / 4);
                _ghostObject.transform.up = hit.normal;
            }

        }



        void PlaceObject(RaycastHit hit)
        {
            if (hit.collider.CompareTag("Interactable")) { _interactable.DisableInteraction(); }

            GameObject placedObject = Instantiate(_interactableObject, _ghostObject.transform.position, Quaternion.identity);
            placedObject.transform.up = hit.normal;  // Rotate to surface normal.
            placedObject.SetActive(true);

            Destroy(_ghostObject);
            Debug.Log(_ghostObject);


            _ghostObject = null;
            _interactableObject = null;
        }

        //Disables outline on item
        void DisableInteraction()
        {
            if (_interactable is null) return;
            _interactable.DisableOutline();
            _interactable = null;
        }

        // Enables interaction outline on item
        void EnableInteraction(Interactable newInteractable)
        {
            if (_interactable == newInteractable) return;
            DisableInteraction();
            _interactable = newInteractable;
            _interactable.EnableOutline();
        }
    }
}