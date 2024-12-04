using UnityEngine;

namespace _Scripts.SceneLogic
{
    public class MySceneBehaviourTest : MonoBehaviour
    {
        void testMyScene() {
            MySceneBehaviour.LoadMyScene(new MySceneParams(/* ... */), (outcome) => {
                Debug.Log("Scene over " + outcome.ToString());
            });
        }
    }
}