using UnityEngine;
using System.Collections.Generic;

namespace Minigame.Fishing
{
    public class Bucket : MonoBehaviour
    {
        public Fish bestFish;
        public List<Fish> fishCaught = new();

        public void AddFish(Fish fish)
        {
            fish.Catch(transform);

            fishCaught.Add(fish);

            if (bestFish == null)
            {
                SetBestFish(fish);
                return;
            }

            if(fish.Difficulty >  bestFish.Difficulty)
            {
                SetBestFish(fish);
            }
        }

        void SetBestFish(Fish fish)
        {
            if(bestFish != null)
                Debug.Log("Prevous Best Fish: " + bestFish.type + "; Size: " + bestFish.Size + "; Weight: " + bestFish.Weight + 
                          " | Best Fish: " + fish.type + "; Size: " + fish.Size + "; Weight: " + fish.Weight);
            bestFish = fish;
        }
    }

}
