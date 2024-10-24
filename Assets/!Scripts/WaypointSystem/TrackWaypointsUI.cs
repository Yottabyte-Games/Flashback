using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.WaypointSystem
{
    public class TrackWaypointsUI : MonoBehaviour {
        [SerializeField] TrackWaypoints trackWaypoints;
        [SerializeField] Image warningImage; 

        void Start() {
            trackWaypoints.OnPlayerCorrectWaypoint += TrackWaypointsOnPlayerCorrectWaypoint;
            trackWaypoints.OnPlayerWrongWaypoint += TrackWaypointsOnPlayerWrongWaypoint;

            Hide();
        }

        #region Show/Hide logic
        void TrackWaypointsOnPlayerWrongWaypoint(object sender, System.EventArgs eventArgs) {
            Debug.Log("TrackWaypointsOnPlayerWrongWaypoint triggered");
            Show();
            warningImage.enabled = true; 
        }

        void TrackWaypointsOnPlayerCorrectWaypoint(object sender, System.EventArgs eventArgs) {
            Debug.Log("TrackWaypointsOnPlayerCorrectWaypoint triggered");
            Hide();
            warningImage.enabled = false;
        }

        void Show() {
            gameObject.SetActive(true);
        }

        void Hide() {
            gameObject.SetActive(false);
        }
        #endregion
    }
}