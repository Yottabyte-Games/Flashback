using UnityEngine;
using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

namespace _Scripts.Singletons
{
    public class SceneSwapper : MonoBehaviour
    {
        [SerializeField] SceneReference mainMenuScene;
        //[SerializeField] SceneReference hubWorldScene;
        //[SerializeField] SceneReference fishingScene;
        //[SerializeField] SceneReference toyCarScene;
        //[SerializeField] SceneReference snowmanScene;
        //[SerializeField] SceneReference workScene;
        [SerializeField] SceneReference fps;
    
        public void MainMenu()
        {
            SceneManager.LoadScene(mainMenuScene.Name);
        }
        /*
        public void HubWorld()
        {
            SceneManager.LoadScene(hubWorldScene.Name);
        }
        public void Fishing()
        {
            SceneManager.LoadScene(fishingScene.Name);
        }
        public void ToyCar()
        {
            SceneManager.LoadScene(toyCarScene.Name);
        }
        public void Snowman()
        {
            SceneManager.LoadScene(snowmanScene.Name);
        }
        public void Work()
        {
            SceneManager.LoadScene(workScene.Name);
        }
                */

        public void FPS()
        {
            SceneManager.LoadScene(fps.Name);
        }

    }
}
