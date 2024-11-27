using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{

    public class MinigameManager : MonoBehaviour
    {
        [Scene, SerializeField] int SceneToLoadOnEnd;
    
        public void EndMinigame()
        {
            SceneManager.LoadScene(SceneToLoadOnEnd);
        }
    }

}
