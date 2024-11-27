using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{

    public class SwapScene : MonoBehaviour
    {
        static bool swapedScene = false;
        void Start()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0 && !swapedScene)
            {
                SceneManager.LoadScene("Begining");
                swapedScene = true;
            }
            else if (SceneManager.GetActiveScene().buildIndex == 13)
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

}
