using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Vehicle.Waypoint
{
    [CreateAssetMenu(fileName = "CircuitData", menuName = "Circuit")]
    public class Circuit : ScriptableObject
    {
        [FormerlySerializedAs("Waypoints")] public Transform[] waypoints;
    }
}
