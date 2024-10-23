using System;
using UnityEngine;

namespace Minigame.Fishing
{
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
    public struct Bait
    {
        public BaitType type;
    }
}
