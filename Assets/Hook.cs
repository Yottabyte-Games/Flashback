using System;
using UnityEngine;

namespace Minigame.Fishing
{
    public class Hook : MonoBehaviour
    {
        public Bait bait;

        public event Action<Fish> caughtFish;

        public void CatchFish(Fish fishCaught)
        {
            caughtFish?.Invoke(fishCaught);
        }
    }

    public enum BaitType
    {
        None = 0,
        Worm = 1,
    }
    [Serializable]
    public class Bait
    {
        public BaitType type;
    }
}