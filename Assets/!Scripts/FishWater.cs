using System.Collections;
using UnityEngine;

namespace _Scripts
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
            var fishNum = Random.Range(0, 25 + 15 * (int)hookInWater.bait.type);

            print(fishNum);
            switch (fishNum)
            {
                case >= 10 + 20 * (int)FishType.GoliathTigerfish: // fishnum > 90
                    hookInWater.CatchFish(new Fish() { type = FishType.GoliathTigerfish });
                    break;

                case >= 10 + 20 * (int)FishType.Sailfish: // fishnum > 70
                    hookInWater.CatchFish(new Fish() { type = FishType.Sailfish });
                    break;

                case >= 10 + 20 * (int)FishType.Bass: // fishnum > 50
                    hookInWater.CatchFish(new Fish() { type = FishType.Bass });
                    break;

                case >= 10 + 20 * (int)FishType.Catfish: // fishnum > 30
                    hookInWater.CatchFish(new Fish() { type = FishType.Catfish });
                    break;

                case >= 10 + 20 * (int)FishType.Panfish: // fishnum > 10
                    hookInWater.CatchFish(new Fish() { type = FishType.Panfish });
                    break;

                case < 10 + 20 * (int)FishType.Panfish: // fishnum < 10
                    StartCoroutine(TryGetFish());
                    break;
            }
        }
    }
}
