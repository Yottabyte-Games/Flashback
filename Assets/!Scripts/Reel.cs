using System;
using Minigame.Fishing;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts
{
    public class Reel : MonoBehaviour
    {
        FishingRodInput input;
        [ReadOnly, SerializeField] bool canReel;
        Fish toReel;

        public event Action finishedReeling;

        private void Start()
        {
            input = GetComponent<FishingRodInput>();
            input.reel += ReelingValue;
        }
        Vector2 lastReelPos, currentReelPos, difference;
        float travelDistance;
        void ReelingValue(Vector2 pos)
        {
            if (!canReel) return;

            if (currentReelPos != null)
                lastReelPos = currentReelPos;

            currentReelPos = pos;

            if(lastReelPos != null && currentReelPos != null)
            {
                difference = currentReelPos - lastReelPos;
            }


            travelDistance += difference.magnitude / 37.7f / 100;

            if(travelDistance > 5 * (int)toReel.type)
            {
                canReel = false;
            }
        }
        public void StartReeling(Fish fishToReel)
        {
            toReel = fishToReel;
            canReel = true;
        }
    }
}
