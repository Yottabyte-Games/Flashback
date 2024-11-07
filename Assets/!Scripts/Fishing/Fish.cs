using System;
using Unity.VisualScripting;
using UnityEngine;
using Utility.Methods;

namespace Minigame.Fishing
{
    public enum FishType
    {
        Trash = 0, 
        Mackerel = 1,//Mackerel 1.5$
        Cod = 2, //4$
        FjordTrout = 3, //6$
        Salmon = 4, //7$
    }

    [Serializable]
    public class Fish : MonoBehaviour
    {
        public FishType type;

        [field: SerializeField, Range(1f, 10f)] public float Weight { get; private set; } = 1;
        [field: SerializeField, Range(1f, 10f)] public float Size { get; private set; } = 1;

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
            Weight = UnityEngine.Random.Range(1f, 10f);
            Size = UnityEngine.Random.Range(1f, 10f);

            if (art == null) return;

            SetVisualSize();
        }
        void SetDifficulty()
        {
            Difficulty = ((float)type * 2.5f + Size + Weight / 2f) * 2f;
        }
        public void Catch(Transform caughtOn)
        {
            transform.parent = caughtOn;
            UMethods.ResetTransform(transform, true);
        }
        void SetVisualSize()
        {
            art.transform.localScale = new Vector3(Weight / 2.5f, Weight / 5f, Size / 5f);
        }
        private void OnDrawGizmosSelected()
        {
            if (art == null) return;

            SetVisualSize();
            SetDifficulty();
        }
    }
}
