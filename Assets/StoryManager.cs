using UnityEngine;

public enum StoryBeat
{
    Introduction,
    Fishing,
    ToyCar,
    Snowman,
    Office,
    Therapist,
    Ending
}

public static class StoryManager
{
    public static StoryBeat StoryBeat { get; private set; }
    public static StoryBeat UpcomingStoryBeat { get { return StoryBeat + 1; } }

    public static void ProgressStory()
    {
        StoryBeat++;
        Debug.Log(StoryBeat);
    }
}
