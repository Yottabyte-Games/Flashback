using UnityEngine;

public class PlayerPositionStorage : MonoBehaviour
{
    public static PlayerPositionStorage Instance; // Singleton instance
    public Vector3 SavedPlayerPosition = Vector3.zero; // Default position if no save exists

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate GameManager instances
        }
    }

}
