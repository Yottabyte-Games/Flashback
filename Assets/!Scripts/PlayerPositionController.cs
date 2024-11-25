using UnityEngine;

public class PlayerPositionController : MonoBehaviour
{
    void Start()
    {
        // Load the player's position from the GameManager if it's set
        if (PlayerPositionStorage.Instance is not null && PlayerPositionStorage.Instance.SavedPlayerPosition != Vector3.zero)
        {
            transform.position = PlayerPositionStorage.Instance.SavedPlayerPosition;
            transform.rotation = PlayerPositionStorage.Instance.SavedPlayerRotation;
            
            Debug.Log($"Loaded position: {transform.position}");
        }
        else
        {
            Debug.Log("No saved position found. Using default position.");
        }
    }

    public void SavePosition()
    {
        // Save the player's current position into the GameManager
        if (PlayerPositionStorage.Instance is null)
            return;
        PlayerPositionStorage.Instance.SavedPlayerPosition = transform.position;
        Debug.Log($"Saved position: {transform.position}");
    }
}
