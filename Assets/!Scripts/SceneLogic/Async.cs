using UnityEngine;

namespace _Scripts.SceneLogic
{
    public class Async : MonoBehaviour
    {
        public void LoadMyScene(MySceneParams sceneParams, System.Action<MySceneOutcome> callback)
        {
            MySceneBehaviour.LoadMyScene(sceneParams, callback);
        }
    }
}
