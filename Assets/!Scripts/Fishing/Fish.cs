using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Minigame.Fishing
{
    public enum FishType
    {
        Trash = 0,
        Panfish = 1,
        Catfish = 2,
        Bass = 3,
        Sailfish = 4,
        GoliathTigerfish = 5,
    }

    [Serializable]
    public class Fish
    {
        [SerializeField] string name;
        public FishType type;
        [field: SerializeField] public float weight { get; private set; }
        [field: SerializeField] public float size { get; private set; }

        [field: SerializeField] public float difficulty { get; private set; }
        public void InitializeFish(FishType type)
        {
            SetFish(type);
            RandomizeFish();
            SetDifficulty(); 
        }
        void RandomizeFish()
        {
            weight = UnityEngine.Random.Range(1f, 5f);
            size = UnityEngine.Random.Range(1f, 5f);
        }
        void SetFish(FishType fish)
        {
            type = fish;
            name = type.ToString();
        }
        void SetDifficulty()
        {
            difficulty = ((float)type * 2 + size + weight / 2) * 2.5f;
        }
    }
}
