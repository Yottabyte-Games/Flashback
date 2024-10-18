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

            InvokeRepeating(nameof(TryGetFish), 1, 5);
        }

        void TryGetFish()
        {
            int fishNum = Random.Range(0, 10 + 20 * (int)hookInWater.bait.type);

            switch (fishNum)
            {
                case > 10 + 20 * (int)FishType.GoliathTigerfish:
                    hookInWater.CatchFish(new Fish() { type = FishType.GoliathTigerfish });
                    break;

                case > 10 + 20 * (int)FishType.Sailfish:
                    hookInWater.CatchFish(new Fish() { type = FishType.Sailfish });
                    break;

                case > 10 + 20 * (int)FishType.Bass:
                    hookInWater.CatchFish(new Fish() { type = FishType.Bass });
                    break;

                case > 10 + 20 * (int)FishType.Catfish:
                    hookInWater.CatchFish(new Fish() { type = FishType.Catfish });
                    break;

                case > 10 + 20 * (int)FishType.Panfish:
                    hookInWater.CatchFish(new Fish() { type = FishType.Panfish });
                    break;

                case < 10 + 20 * (int)FishType.Panfish:
                    break;
            }
        }
    }
}
