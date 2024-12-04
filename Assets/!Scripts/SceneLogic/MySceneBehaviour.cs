using UnityEngine;
using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

namespace _Scripts.SceneLogic
{
    public class MySceneBehaviour: MonoBehaviour {
        static MySceneParams _loadSceneRegister;
        [SerializeField] MySceneParams sceneParams;
        
        [SerializeField] SceneReference myScene;
        static SceneReference _myScene;

        public static void LoadMyScene(MySceneParams sceneParams, System.Action<MySceneOutcome> callback) {
            _loadSceneRegister = sceneParams;
            sceneParams.Callback = callback;
            SceneManager.LoadScene(_myScene.Name);
        }

        public void Awake() {
            if (_loadSceneRegister != null) sceneParams = _loadSceneRegister;
            _myScene = myScene;
            _loadSceneRegister = null; // the register has served its purpose, clear the state
        }

        public void EndScene (MySceneOutcome outcome) {
            if (sceneParams.Callback is not null) sceneParams.Callback(outcome);
            sceneParams.Callback = null; // Protect against double calling;
        }
    }

    [System.Serializable]
    public class MySceneParams {
        public System.Action<MySceneOutcome> Callback;
        // + inputs of the scene 
    }

    public class MySceneOutcome {
        // + outputs of the scene 
    }
}