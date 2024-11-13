using Eflatun.SceneReference;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.SceneLogic
{
    [ShowOdinSerializedPropertiesInInspector]
    public class MySceneBehaviour: MonoBehaviour {
        [OdinSerialize] static MySceneParams _loadSceneRegister;
        [OdinSerialize] MySceneParams sceneParams;
        
        [OdinSerialize] SceneReference myScene;
        [OdinSerialize] static SceneReference _myScene;

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