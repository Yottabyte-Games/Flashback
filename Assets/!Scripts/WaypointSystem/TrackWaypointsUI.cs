using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.WaypointSystem
{
    public class TrackWaypointsUI : MonoBehaviour {
        [SerializeField] TrackWaypoints trackWaypoints;
        [SerializeField] Image warningImage; 

        void Start() {
            SubscribeToEvents();
            SetVisibility(false);
        }

        #region Show/Hide logic
        void TrackWaypointsOnPlayerWrongWaypoint(object sender, System.EventArgs eventArgs) {
            Debug.Log("TrackWaypointsOnPlayerWrongWaypoint triggered");
            SetVisibility(true);
            warningImage.enabled = true;
        }

        void TrackWaypointsOnPlayerCorrectWaypoint(object sender, System.EventArgs eventArgs) {
            Debug.Log("TrackWaypointsOnPlayerCorrectWaypoint triggered");
            SetVisibility(false);
            warningImage.enabled = false;
        }

        void SetVisibility(bool isVisible) {
            gameObject.SetActive(isVisible);
        }

        void SubscribeToEvents() {
            trackWaypoints.OnPlayerCorrectWaypoint += TrackWaypointsOnPlayerCorrectWaypoint;
            trackWaypoints.OnPlayerWrongWaypoint += TrackWaypointsOnPlayerWrongWaypoint;
        }
        #endregion
    }
}