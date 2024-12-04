using UnityEngine;
using UnityEngine.Events;
namespace _Scripts
{
    public class StoryProgresser : MonoBehaviour
    {
        [SerializeField] StoryBeat ActOnBeat;
        [SerializeField] StoryBeat ChangeToBeat;
        public UnityEvent<StoryProgresser> OnProgressedStory;

        void OnTriggerEnter(Collider other)
        {
            SelectNextStoryBeat();
        }

        public void SelectNextStoryBeat()
        {
            print(StoryManager.StoryBeat + " " + ActOnBeat);
            if(StoryManager.StoryBeat != ActOnBeat) return;

            StoryManager.ProgressStory(ChangeToBeat);
            OnProgressedStory?.Invoke(this);
            Destroy(this);
        }
    }
}
