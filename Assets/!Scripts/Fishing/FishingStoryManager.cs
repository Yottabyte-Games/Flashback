using _Scripts.Fishing;
using NaughtyAttributes;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Fishing
{
    public class FishingStoryManager : MonoBehaviour
    {
        [SerializeField] FishDisplayer fishDisplayer;

        [SerializeField] int fishCaught { get { return fishDisplayer.fishCaught.Count; } }
        [SerializeField] int amountOfFishNeeded;

        [Scene]
        [SerializeField] int storyScene;

        [SerializeField] TMP_Text fishLeftText;

        private void Start()
        {
            fishLeftText.text = fishCaught + "/" + amountOfFishNeeded;
            fishDisplayer.FishAdded += CanProgressStory;
        }

        private void CanProgressStory()
        {
            fishLeftText.text = fishCaught + "/" + amountOfFishNeeded;

            if (fishCaught < amountOfFishNeeded) return;

            ProgressStory();
        }

        private void ProgressStory()
        {
            SceneManager.LoadScene(storyScene);
        }
    }
}
