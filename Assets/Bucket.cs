using System.Collections.Generic;
using UnityEngine;
using Utility.Methods;
using static UnityEditor.FilePathAttribute;

namespace Minigame.Fishing
{
    public class Bucket : MonoBehaviour
    {
        public Fish bestFish;
        public List<Fish> fishCaught = new();

        [SerializeField] Transform bestFishLocation;
        [SerializeField] List<Transform> fishLocations = new();

        private void OnTriggerEnter(Collider other)
        {
            
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

            print("added");

            UpdateFishLocations();
        }

        void SetBestFish(Fish fish)
        {
            if (bestFish != null)
                Debug.Log("Prevous Best Fish: " + bestFish.type + "; Size: " + bestFish.Size + "; Weight: " + bestFish.Weight +
                          " | Best Fish: " + fish.type + "; Size: " + fish.Size + "; Weight: " + fish.Weight);
            bestFish = fish;
        }

        void UpdateFishLocations()
        {
            //set all fishes to be invisible and in the bucket
            foreach (var fish in fishCaught)
            {
                fish.Catch(transform);
                fish.gameObject.SetActive(false);
            }

            //set the last 4 fishes to be on fish locations
            bool foundBestFish = false;

            for (int i = 0; i < fishLocations.Count; i++)
            {
                if (fishCaught.Count > i)
                {
                    Fish fishToPlace = foundBestFish ? fishCaught[^(i + 2)] : fishCaught[^(i + 1)];

                    if (fishToPlace != bestFish)
                    {
                        fishToPlace.transform.parent = fishLocations[i];
                    }
                    else if(fishCaught.Count > i + 2)
                    {
                        fishCaught[^(i + 2)].transform.parent = fishLocations[i];
                        foundBestFish = true;
                    }
                }
            }

            //set the best fish on the best fish location
            if(bestFish != null)
            {
                bestFish.transform.parent = bestFishLocation;
            }

            foreach (var location in fishLocations)
            {
                if(location.childCount > 0)
                {
                    Transform fish = location.GetChild(0);

                    UMethods.ResetTransform(fish, true);
                    fish.gameObject.SetActive(true);
                }
            }
            if (bestFishLocation.childCount > 0)
            {
                Transform fish = bestFishLocation.GetChild(0);

                UMethods.ResetTransform(fish, true);
                fish.gameObject.SetActive(true);
            }
        }
    }

}
