using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace _Scripts
{
    public class PlayerPositionController : MonoBehaviour
    {
        [SerializeField] bool isWorking = false;
        void Start()
        {
            if (!isWorking)
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
        }

        public void SavePosition(string sceneName)
        {
            // Save the player's current position into the GameManager
            if (PlayerPositionStorage.Instance is null)
                return;
        
            StartCoroutine(TransitionToScene(sceneName));
        }

        IEnumerator TransitionToScene(string sceneName)
        {
            PlayerPositionStorage.Instance.SavedPlayerPosition = transform.position;
            Debug.Log($"Saved position: {transform.position}");
            yield return new WaitForSeconds(1f); // Ensure position is saved
            SceneManager.LoadScene(sceneName);
        }
    }
}
