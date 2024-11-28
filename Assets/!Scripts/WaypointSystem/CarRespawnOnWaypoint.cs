using UnityEngine;
namespace _Scripts.WaypointSystem {
    
    [RequireComponent(typeof(TrackWaypointManager))]
    public class CarRespawnOnWaypoint : MonoBehaviour {
        
        [SerializeField] Transform carTransform;

        TrackWaypointManager _trackWaypointManager;

        void Awake() {
            _trackWaypointManager = GetComponent<TrackWaypointManager>();
        }

        public void RespawnCar() {
            if (HasPassedWaypoint()) {
                SingleWaypoint currentWaypoint = GetPreviousActiveWaypoint();
                carTransform.position = currentWaypoint.transform.position;
                carTransform.rotation = currentWaypoint.transform.rotation;
            } else {
                Debug.LogWarning("Player has not passed the waypoint yet. Respawn not allowed.");
            }
        }
        
        SingleWaypoint GetPreviousActiveWaypoint()
        {
            int currentWaypointIndex = _trackWaypointManager.NextWaypointSingleIndexList[_trackWaypointManager.CarIndex];
            return currentWaypointIndex > 0 ? _trackWaypointManager.WaypointSingleList[currentWaypointIndex - 1] : _trackWaypointManager.WaypointSingleList[0];
        }

        bool HasPassedWaypoint() {
            int currentWaypointIndex = _trackWaypointManager.NextWaypointSingleIndexList[_trackWaypointManager.CarIndex];
            return currentWaypointIndex > 0;
        }
    }
}
