using Eflatun.SceneReference;
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

        //sorry for ass refactoring and code changes, but we can fix this later
        [SerializeField] SceneReference sceneOnSuccess;

        public void CheckSucceess(int tasks)
        {
            Succeeded = tasks >= tasksGoal;
            if(Succeeded)
            {
                SceneManager.LoadScene(sceneOnSuccess.Name);
            } 
        }
        public void CheckFailure(int tasks)
        {
            if(tasks >= tasksLimit)
            {
                if(Succeeded)
                {
                    SceneManager.LoadScene(sceneOnSuccess.Name);
                } else
                {
                    //Just so executives progresses if they fail for this build
                    SceneManager.LoadScene(sceneOnSuccess.Name);
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }
}
