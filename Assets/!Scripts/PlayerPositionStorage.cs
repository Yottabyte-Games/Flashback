using _Scripts.Generic;
using UnityEngine;

namespace _Scripts
{

    public class PlayerPositionStorage : Singleton<PlayerPositionStorage>
    {
        public Vector3 SavedPlayerPosition = Vector3.zero; // Default position if no save exists
        public Quaternion SavedPlayerRotation;             // Default rotation if no save exists
    }

}
