using UnityEngine;

public class Interactable : MonoBehaviour
{
    public int sceneIndex;
    public Vector3 targetRotation;
    // Skriv en melding eller lag en handling som skjer ved interaksjon
    public virtual void Interact()
    {
        Debug.Log(gameObject.name);
    }
}