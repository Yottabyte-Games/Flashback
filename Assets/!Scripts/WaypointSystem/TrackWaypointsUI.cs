using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.WaypointSystem
{
    public class TrackWaypointsUI : MonoBehaviour {
        #region Declarations
        [SerializeField] TrackWaypoints trackWaypoints;
        [SerializeField] Image warningImage;
        #endregion

        void Start() {
            SubscribeToEvents();
            SetVisibility(false);
        }

        #region Show/Hide logic
        void TrackWaypointsOnPlayerWrongWaypoint(object sender, System.EventArgs eventArgs) {
            SetVisibility(true);
            warningImage.enabled = true;
        }

        void TrackWaypointsOnPlayerCorrectWaypoint(object sender, System.EventArgs eventArgs) {
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