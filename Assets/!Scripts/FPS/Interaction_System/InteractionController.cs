using _Scripts.FPS.Scriptable_Objects;
using UnityEngine;

namespace _Scripts.FPS.Interaction_System
{    
    public class InteractionController : MonoBehaviour
    {
        #region Variables    
            [Space, Header("Data")]
            [SerializeField] private InteractionInputData interactionInputData = null;
            [SerializeField] private InteractionData interactionData = null;

            [Space, Header("UI")]
            [SerializeField] private InteractionUIPanel uiPanel;

            [Space, Header("Ray Settings")]
            [SerializeField] private float rayDistance = 0f;
            [SerializeField] private float raySphereRadius = 0f;
            [SerializeField] private LayerMask interactableLayer = ~0;

            bool _hitSomething;

            public int layer;


        #region Private
                [SerializeField] private Camera m_cam;

                private bool m_interacting;
                private float m_holdTimer = 0f;

        #endregion

        #endregion

        #region Built In Methods      
            /*void Awake()
            {
                //m_cam = FindObjectOfType<Camera>();
                //m_cam = Object.FindFirstObjectOfType<Camera>;
            }*/

            void Update()
            {
                CheckForInteractable();
                CheckForInteractableInput();
            }
        #endregion


        #region Custom methods         
        void CheckForInteractable()
        {
            Ray _ray = new Ray(m_cam.transform.position, m_cam.transform.forward);
            RaycastHit _hitInfo;

            _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactableLayer);
            Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);

            if (_hitSomething) 
            {
                InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();

                if (_interactable != null)
                {
                    //Debug.Log("Hit: " + _hitInfo.collider.name);

                    if (interactionData.IsEmpty())
                    {
                        interactionData.Interactable = _interactable;
                        layer = _interactable.gameObject.layer;
                    }
                    else
                    {
                        if (!interactionData.IsSameInteractable(_interactable))
                        {
                            //uiPanel.SetTooltip(_interactable.TooltipMessage);
                            //return;
                        }
                        else
                            uiPanel.SetTooltip(_interactable.TooltipMessage);
                        //else
                        //     interactionData.Interactable = _interactable;

                    }

                    //if (_interactable.TooltipMessage == "Equip")
                    //    uiPanel.SetTooltip("Equip");

                }
                else
                {
                    interactionInputData.ResetInput();
                }
            }
            else
            {
                interactionData.ResetData();
                uiPanel.ResetUI();

                // if the raycast does not hit the interactable object, zero the holdTimer
                m_holdTimer = 0f;
            }


            /*if (_hitSomething)
            {
                InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();

                if (_interactable != null)
                {
                    if (interactionData.IsEmpty())
                    {
                        interactionData.Interactable = _interactable;
                        uiPanel.SetTooltip("Interact");
                    }
                    else
                    {
                        if (!interactionData.IsSameInteractable(_interactable))
                            return; }
                        else     
                            interactionData.Interactable = _interactable;
                        
                        if (interactionData.IsSameInteractable(_interactable))
                            return;
                        else
                            interactionData.Interactable = _interactable;
                    }
                }
                else
                {
                    uiPanel.enabled = false;
                    uiPanel.ResetUI();
                    interactionData.ResetData();

                    // Set hold time to zero so the Progress bar is not remembering the previous progress of the hold button
                    m_holdTimer = 0f;
                }
                

                //Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
                //uiPanel.SetTooltip("Interact");
            }*/
        }

        void CheckForInteractableInput()
        {
            if(interactionData.IsEmpty())
                return;

            if(interactionInputData.InteractedClicked)
            {
                m_interacting = true;
                m_holdTimer = 0f;
            }

            if(interactionInputData.InteractedReleased)
            {   
                m_interacting = false;
                m_holdTimer = 0f;
                uiPanel.UpdateProgressBar(0f);
            }

            if(m_interacting && _hitSomething)
            {
                if(!interactionData.Interactable.IsInteractable)
                    return;

                if(interactionData.Interactable.HoldInteract)
                {
                    m_holdTimer += Time.deltaTime;

                    float heldPercent = m_holdTimer / interactionData.Interactable.HoldDuration;
                    uiPanel.UpdateProgressBar(heldPercent);

                    if(heldPercent > 1f)
                    {
                        interactionData.Interact();
                        m_interacting = false;
                    }
                }
                else
                {
                    interactionData.Interact();
                    m_interacting = false;
                }
            }
            else
                uiPanel.UpdateProgressBar(0f);
        }
        #endregion
    }
}
