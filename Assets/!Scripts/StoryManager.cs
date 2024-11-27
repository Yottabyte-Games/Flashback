using UnityEngine;

namespace _Scripts
{

    public enum StoryBeat
    {
        None,
        Introduction,
        Reroute,
        Fishing,
        FinishedFishing,
        ToyCar,
        FinishedToyCar,
        Snowman,
        FinishedSnowman,
        Office,
        FinishedOffice,
        Therapist,
        FinishedTherapist,
        Ending
    }

    public static class StoryManager
    {
        public static StoryBeat StoryBeat { get; private set; }
        public static StoryBeat UpcomingStoryBeat { get { return StoryBeat + 1; } }

        public static void ProgressStory(StoryBeat storyBeat)
        {
            StoryBeat = storyBeat;
            Debug.Log(StoryBeat);
        }
    }

}