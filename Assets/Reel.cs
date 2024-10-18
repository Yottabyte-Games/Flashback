using NaughtyAttributes;
using UnityEngine;

namespace Minigame.Fishing
{
    public class Reel : MonoBehaviour
    {
        [SerializeField] bool canReel;
        public void StartReeling(Fish fishToReel)
        {
            canReel = true;
        }
    }
}
