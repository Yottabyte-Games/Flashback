using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEBUG_CarScene : MonoBehaviour
{
    [SerializeField] SceneReference hubWorld;
    // Update is called once per frame
    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.L))
            return;
        SceneManager.LoadScene(hubWorld.BuildIndex);
    }
}
