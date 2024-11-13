using _Scripts.Fishing;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
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
