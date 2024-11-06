using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Fishing
{
    public class FishWater : MonoBehaviour
    {
        [SerializeField] List<Fish> fishList = new List<Fish>();

        Hook _hookInWater;

        private void OnTriggerEnter(Collider other)
        {
            if (_hookInWater != null) return;
            _hookInWater = other.GetComponent<Hook>();

            StartCoroutine(TryGetFish());
        }

        IEnumerator TryGetFish()
        {
            yield return new WaitForSeconds(5);
            int fishNum = Random.Range(0, 25 + 15 * (int)_hookInWater.bait.type);

            Fish fish = Instantiate(fishList[Mathf.RoundToInt(fishNum / 20)].gameObject).GetComponent<Fish>();

            if (fish.type != FishType.Trash)
            {
                _hookInWater.CatchFish(fish);
            }
            else
            {
                StartCoroutine(TryGetFish());
            }
        }
    }
}
