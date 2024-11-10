using UnityEngine;
using _Scripts.Generic;
using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

namespace _Scripts.Singletons
{
    public class SceneSwapper : Singleton<SceneSwapper>
    {
        [SerializeField] SceneReference hubWorldScene;
        [SerializeField] SceneReference fishingScene;
        [SerializeField] SceneReference toyCarScene;
        [SerializeField] SceneReference snowmanScene;
    
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
        

    }
}
