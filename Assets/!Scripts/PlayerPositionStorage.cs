using UnityEngine;
using _Scripts.Generic;
namespace _Scripts
{
    public class PlayerPositionStorage : Singleton<PlayerPositionStorage>
    {
        public Vector3 SavedPlayerPosition = Vector3.zero; // Default position if no save exists
    }
}
