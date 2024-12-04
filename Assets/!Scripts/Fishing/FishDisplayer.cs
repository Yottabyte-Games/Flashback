using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Fishing
{
    public class FishDisplayer : MonoBehaviour
    {
        [SerializeField] FishingRod fishingRod;

        [Space]
        public Fish bestFish;
        public List<Fish> fishCaught = new();

        [SerializeField] FishDisplay bestFishLocation;
        [SerializeField] List<FishDisplay> fishLocations = new();

        public event Action FishAdded;

        void OnTriggerEnter(Collider other)
        {
            if (fishingRod.hook.fish == null) return;
            if (other.gameObject.layer != 9) return;

            AddFish(fishingRod.hook.fish);
            fishingRod.hook.fish = null;
        }

        public void AddFish(Fish fish)
        {
            fishCaught.Add(fish);

            if (bestFish == null)
            {
                SetBestFish(fish);
            }
            else if (fish.Difficulty > bestFish.Difficulty)
            {
                SetBestFish(fish);
            }

            UpdateFishLocations();
            FishAdded.Invoke();
        }

        void SetBestFish(Fish fish)
        {
            if (bestFish != null)
                Debug.Log("Prevous Best Fish: " + bestFish.type + "; Size: " + bestFish.Length + "; Weight: " + bestFish.Weight +
                          " | Best Fish: " + fish.type + "; Size: " + fish.Length + "; Weight: " + fish.Weight);
            bestFish = fish;
        }

        void UpdateFishLocations()
        {
            //set all fishes to be invisible and in the bucket
            foreach (var loc in fishLocations)
            {
                loc.RemoveFish();
            }

            //set the last 4 fishes to be on fish locations
            bool foundBestFish = false;

            for (int i = 0; i < fishLocations.Count; i++)
            {
                if (fishCaught.Count > i)
                {
                    Fish fishToPlace = foundBestFish ? fishCaught[^(i + 2)] : fishCaught[^(i + 1)];
                    fishLocations[i].DisplayFish(fishToPlace);

                    //if (fishToPlace == null)
                    //    break;

                    //if (fishToPlace != bestFish)
                    //{
                    //    fishLocations[i].DisplayFish(fishToPlace);
                    //}
                    //else if (fishCaught.Count > i + 2)
                    //{
                    //    if (fishCaught[^(i + 2)] == null)
                    //        break;

                    //    fishLocations[i].DisplayFish(fishCaught[^(i + 2)]);

                    //    foundBestFish = true;
                    //}
                }
            }

            //set the best fish on the best fish location
            if (bestFish != null)
            {
                bestFishLocation.DisplayFish(bestFish);
            }
        }
    }

}
