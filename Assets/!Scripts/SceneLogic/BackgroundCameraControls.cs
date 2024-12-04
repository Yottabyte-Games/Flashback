using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackgroundCameraControls : MonoBehaviour
{
    [SerializeField] SceneReference sceneToLoad;
    void Start()
    {
        SceneManager.LoadScene(sceneToLoad.Name, LoadSceneMode.Additive);
    }
}
