using System.Collections.Generic;
using UnityEngine;
using Utility.Methods;

namespace Minigame.Fishing
{
    public class Bucket : MonoBehaviour
    {
        public Fish bestFish;
        public List<Fish> fishCaught = new();

        [SerializeField] Transform bestFishLocation;
        [SerializeField] List<Transform> fishLocations = new();

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

            PlaceFish(fish);
        }

        void SetBestFish(Fish fish)
        {
            if (bestFish != null)
                Debug.Log("Prevous Best Fish: " + bestFish.type + "; Size: " + bestFish.Size + "; Weight: " + bestFish.Weight +
                          " | Best Fish: " + fish.type + "; Size: " + fish.Size + "; Weight: " + fish.Weight);
            bestFish = fish;
        }

        void PlaceFish(Fish fish)
        {

            print("hello");
            if (fish == bestFish)
            {
                if (bestFishLocation.childCount > 0)
                {
                    Transform previousFish = bestFishLocation.GetChild(0);
                    previousFish.gameObject.SetActive(false);
                    previousFish.parent = transform;
                }
                fish.Catch(bestFishLocation);

                return;
            }

            for (int i = 0; i < fishLocations.Count; i++)
            {
                if (fishLocations[i].childCount > 0)
                {
                    bool shouldContinue = false;

                    Transform previousFish = fishLocations[i].GetChild(0);

                    print(i + 1 < fishLocations.Count);

                    if (i + 1 < fishLocations.Count)
                    {
                        if (fishLocations[i + 1].childCount > 0)
                        {
                            shouldContinue = true;
                        }
                        previousFish.transform.parent = fishLocations[i + 1];
                        UMethods.ResetTransform(previousFish);
                    }
                    else
                    {
                        previousFish.gameObject.SetActive(false);
                        previousFish.parent = transform;
                    }

                    if (shouldContinue)
                        break;
                }
            }


            fish.Catch(fishLocations[0]);
        }
    }

}
