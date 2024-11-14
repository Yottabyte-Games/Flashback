using NaughtyAttributes;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace _Scripts.Fishing
{
    public class CutsceneManger : MonoBehaviour
    {
        [SerializeField] PlayableDirector director;
        [Scene]
        [SerializeField] int nextScene;
        [SerializeField] float sceneChangeDelay;

        private void Start()
        {
            director.Play();
            director.stopped += NextScene;
        }

        async void NextScene(PlayableDirector director)
        {
            await Task.Delay(Mathf.RoundToInt(sceneChangeDelay * 1000));
            SceneManager.LoadScene(nextScene);
        }
    }
}
