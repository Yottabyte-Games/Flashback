using Unity.VisualScripting;
using UnityEngine;

namespace Minigame.Fishing
{
    public enum FishType
    {
        None = 0,
        Panfish = 1,
        Catfish = 2,
        Bass = 3,
        Sailfish = 4,
        GoliathTigerfish = 5,
    }

    public struct Fish
    {
        public FishType type;
        public float difficultyModifier;

        public float difficulty
        {
            get
            {
                return ((float)type * 2 + difficultyModifier) * 2.5f;
            }
        }

        public void RandomizeFish()
        {
            difficultyModifier = Random.Range(1f, 5f);
        }
        public void SetFish(FishType fish)
        {
            type = fish;
        }
    }
}
