using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame.Fishing
{
    public class FishWater : Water
    {
        [SerializeField] List<Fish> fishList = new();

        readonly List<Hook> hooksInWater = new();


        private void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out Hook hookInWater))
            {
                if (InWater(hookInWater)) return;

                AddHook(hookInWater);

                if (hookInWater.fish == null)
                    StartCoroutine(TryGetFish(hookInWater));
            }

            print(hookInWater);
        }

        IEnumerator TryGetFish(Hook hook)
        {
            yield return new WaitForSeconds(5);

            if (InWater(hook))
            {
                int fishNum = Random.Range(0, 25 + 15 * (int)hook.bait.type);

                Fish fish = Instantiate(fishList[Mathf.RoundToInt(fishNum / 20)].gameObject).GetComponent<Fish>();

                if (fish.type != FishType.Trash)
                {
                    hook.CatchFish(fish);
                }
                else
                {
                    StartCoroutine(TryGetFish(hook));
                }
            }
        }
        public bool InWater(Hook hook)
        {
            if(hooksInWater.Count == 0) return false;

            foreach (var item in hooksInWater)
            {
                if(item == hook)
                {
                    return true;
                }
            }

            return false;
        }
        public void AddHook(Hook hook)
        {
            hook.water = this;
            hooksInWater.Add(hook);
        }
        public void RemoveHook(Hook hook)
        {
            hook.water = null;
            hooksInWater.Remove(hook);
        }
    }
}
