using NaughtyAttributes;
using UnityEngine;

namespace Minigame.Fishing
{
    public class Reel : MonoBehaviour
    {
        FishingRodInput input;
        [ReadOnly, SerializeField] bool canReel;

        private void Start()
        {
            input = GetComponent<FishingRodInput>();
            input.reel += ReelingValue;
        }
        private void Update()
        {
            if (!canReel) return;
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
        }
        public void StartReeling(Fish fishToReel)
        {
            canReel = true;
        }
    }
}
