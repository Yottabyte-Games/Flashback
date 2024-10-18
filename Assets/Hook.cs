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
            print(fishCaught.type);
            caughtFish?.Invoke(fishCaught);
        }
    }

    public enum BaitType
    {
        None = 0,
        Bread = 1,
        Worm = 2,
        Leech = 3,
        ChickenLiver = 4,
        Shrimp = 5,
    }
    [Serializable]
    public class Bait
    {
        public BaitType type;
    }
}