using System;
using Unity.VisualScripting;
using UnityEngine;
using Utility.Methods;

namespace Minigame.Fishing
{
    public enum FishType
    {
        Trash = 0,
        Panfish = 1,
        RedTailCatfish = 2,
        Bass = 3,
        Sailfish = 4,
        GoliathTigerfish = 5,
    }

    [Serializable]
    public class Fish : MonoBehaviour
    {
        public FishType type;

        [field: SerializeField, Range(1f, 5f)] public float Weight { get; private set; } = 1;
        [field: SerializeField, Range(1f, 5f)] public float Size { get; private set; } = 1;

        [field: SerializeField] public float Difficulty { get; private set; }

        [Space]
        [SerializeField] Transform art;
        public void Start()
        {
            RandomizeFish();
            SetDifficulty(); 
        }
        void RandomizeFish()
        {
            Weight = UnityEngine.Random.Range(1f, 5f);
            Size = UnityEngine.Random.Range(1f, 5f);

            if (art == null) return;

            SetVisualSize();
        }
        void SetDifficulty()
        {
            Difficulty = ((float)type * 2 + Size + Weight / 1.5f) * 2.5f;
        }
        public void Catch(Transform caughtOn)
        {
            transform.parent = caughtOn;
            UMethods.ResetTransform(transform, true);
        }
        void SetVisualSize()
        {
            art.transform.localScale = new Vector3(Weight / 1.25f, Weight / 2.5f, Size / 2.5f);
        }
        private void OnDrawGizmosSelected()
        {
            if (art == null) return;

            SetVisualSize();
        }
    }
}
