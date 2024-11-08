using System;
using UnityEngine;

namespace _Scripts.Fishing
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
    public class Fish : MonoBehaviour
    {
        public FishType type;
        [field: SerializeField] public float weight { get; private set; } = 1;
        [field: SerializeField] public float size { get; private set; } = 1;

        [field: SerializeField] public float difficulty { get; private set; }

        [Space]
        [SerializeField] Transform art;
        public void Start()
        {
            RandomizeFish();
            SetDifficulty(); 
        }
        void RandomizeFish()
        {
            weight = UnityEngine.Random.Range(1f, 5f);
            size = UnityEngine.Random.Range(1f, 5f);

            if (art == null) return;

            art.transform.localScale = new Vector3(weight / 4, art.transform.localScale.y, size * 2);
        }
        void SetDifficulty()
        {
            difficulty = ((float)type * 2 + size + weight / 2) * 2.5f;
        }

        void OnDrawGizmosSelected()
        {
            if (art == null) return;

            art.transform.localScale = new Vector3(weight / 4, art.transform.localScale.y, size * 2);
        }
    }
}
