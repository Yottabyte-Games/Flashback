using UnityEngine;

namespace _Scripts.SceneLogic
{
    public class async : MonoBehaviour
    {
        void loadMyScene(MySceneParams params, System.Action<MySceneOutcome> callback);
    }
}