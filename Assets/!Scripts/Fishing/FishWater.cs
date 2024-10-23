using System.Collections;
using UnityEngine;

namespace Minigame.Fishing
{
    public class FishWater : MonoBehaviour
    {
        Hook hookInWater;

        private void OnTriggerEnter(Collider other)
        {
            if (hookInWater != null) return;
            hookInWater = other.GetComponent<Hook>();

            StartCoroutine(TryGetFish());
        }

        IEnumerator TryGetFish()
        {
            yield return new WaitForSeconds(5);
            int fishNum = Random.Range(0, 25 + 15 * (int)hookInWater.bait.type);

            Fish fish = new Fish();
            fish.RandomizeFish();
            fish.SetFish((FishType)Mathf.RoundToInt(fishNum / 20));

            if(fish.type != FishType.None)
            {
                print(fish.type);
                hookInWater.CatchFish(fish);
            } else
            {
                print("restart");
                StartCoroutine(TryGetFish());
            }
        }
    }
}
