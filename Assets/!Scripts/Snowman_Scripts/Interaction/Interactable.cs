using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    private Rigidbody rb;
    public UnityEvent onInteract;
    Outline outline;
    public bool canInteract = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
        //Disable Outline
    }

    public void Interact()
    {
        if(canInteract)
        {
            rb.isKinematic = true;
            gameObject.SetActive(false);
            onInteract?.Invoke();   
        }
    }

    public void DisableOutline()
    {
        outline.enabled = false;
    }

    public void EnableOutline()
    {
        //Debug.Log("Enabled Outline");
        outline.enabled = true;
    }

    public void DisableInteraction()
    {
        canInteract = false;
        rb.isKinematic = true;
    }
}
