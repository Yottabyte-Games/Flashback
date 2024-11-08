using UnityEngine;

namespace GitAmendKartSample.Scripts {
    [CreateAssetMenu(fileName = "CircuitData", menuName = "Kart/CircuitData")]
    public class Circuit : ScriptableObject {
        public Transform[] waypoints;
        public Transform[] spawnPoints;
    }
}