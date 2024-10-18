using UnityEngine;

namespace Minigame.Fishing
{
    public class Reel : MonoBehaviour
    {
        Casting cast;

        private void Start()
        {
            cast = GetComponent<Casting>();
            cast.hook.GetComponent<Hook>().caughtFish += StartReeling;
        }

        void StartReeling(Fish fishToReel)
        {

        }
    }
}
