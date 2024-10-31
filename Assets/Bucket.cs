using UnityEngine;
using System.Collections.Generic;

namespace Minigame.Fishing
{
    public class Bucket : MonoBehaviour
    {
        public Fish bestFish;
        public List<Fish> fishCaught = new List<Fish>();

        public void AddFish(Fish fish)
        {
            fishCaught.Add(fish);

            if (bestFish == null)
            {
                SetBestFish(fish);
                return;
            }

            if(fish.difficulty >  bestFish.difficulty)
            {
                SetBestFish(fish);
            }
        }

        void SetBestFish(Fish fish)
        {
            if(bestFish != null)
                Debug.Log("Prevous Best Fish: " + bestFish.type + "; Size: " + bestFish.size + "; Weight: " + bestFish.weight + 
                          " | Best Fish: " + fish.type + "; Size: " + fish.size + "; Weight: " + fish.weight);
            bestFish = fish;
        }
    }

}
