using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Working
{
    public class OfficeStoryManager : MonoBehaviour
    {
        public bool Succeeded;
        public int tasksGoal;
        public int tasksLimit;

        [Scene]
        [SerializeField] string sceneOnSuccess;

        public void CheckSucceess(int tasks)
        {
            Succeeded = tasks >= tasksGoal;
        }
        public void CheckFailure(int tasks)
        {
            if(tasks >= tasksLimit)
            {
                if(Succeeded)
                {
                    SceneManager.LoadScene(sceneOnSuccess);
                } else
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }
}
