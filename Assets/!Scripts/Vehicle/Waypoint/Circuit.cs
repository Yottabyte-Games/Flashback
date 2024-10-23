using UnityEngine;

namespace _Scripts.Vehicle.Waypoint
{
    [CreateAssetMenu(fileName = "CircuitData", menuName = "Circuit")]
    public class Circuit : ScriptableObject
    {
        public Transform[] Waypoints;
    }
}
