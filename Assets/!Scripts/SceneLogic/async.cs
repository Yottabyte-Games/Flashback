using UnityEngine;

namespace _Scripts.SceneLogic
{
    public class async : MonoBehaviour
    {
        public void loadMyScene(MySceneParams sceneParams, System.Action<MySceneOutcome> callback)
        {
            MySceneBehaviour.LoadMyScene(sceneParams, callback);
        }
    }
}
