using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.WaypointSystem
{
    public class TrackWaypointsUI : MonoBehaviour {
        #region Declarations
        [SerializeField] TrackWaypointManager trackWaypointManager;
        [SerializeField] Image warningImage;
        #endregion

        void Start() {
            SubscribeToEvents();
            SetVisibility(false);
        }

        #region Show/Hide logic
        void TrackWaypointManagerOnPlayerWrongWaypointManager(object sender, System.EventArgs eventArgs) {
            SetVisibility(true);
            warningImage.enabled = true;
        }

        void TrackWaypointManagerOnPlayerCorrectWaypointManager(object sender, System.EventArgs eventArgs) {
            SetVisibility(false);
            warningImage.enabled = false;
        }

        void SetVisibility(bool isVisible) {
            gameObject.SetActive(isVisible);
        }

        void SubscribeToEvents() {
            trackWaypointManager.OnPlayerCorrectWaypoint += TrackWaypointManagerOnPlayerCorrectWaypointManager;
            trackWaypointManager.OnPlayerWrongWaypoint += TrackWaypointManagerOnPlayerWrongWaypointManager;
        }
        #endregion
    }
}