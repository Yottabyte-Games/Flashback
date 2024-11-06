using System.Collections.Generic;
using _Scripts.Fishing;
using UnityEngine;

namespace _Scripts
{
    public class Bucket : MonoBehaviour
    {
        public Fish bestFish;
        public List<Fish> fishCaught = new List<Fish>();

        public void AddFish(Fish fish)
        {
            fish.transform.parent = transform;
            fish.transform.localPosition = Vector3.zero;
            fish.transform.localEulerAngles = Vector3.zero;

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
