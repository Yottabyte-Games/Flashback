using _Scripts.Fishing;
using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Fishing
{
    public class FishingStoryManager : MonoBehaviour
    {
        [SerializeField] FishDisplayer fishDisplayer;
        [SerializeField] int amountOfFishNeeded;

        [Scene]
        [SerializeField] int storyScene;

        private void Start()
        {
            fishDisplayer.FishAdded += CanProgressStory;
        }

        private void CanProgressStory()
        {
            if (fishDisplayer.fishCaught.Count <= amountOfFishNeeded) return;

            ProgressStory();
        }

        private void ProgressStory()
        {
            SceneManager.LoadScene(storyScene);
        }
    }
}
