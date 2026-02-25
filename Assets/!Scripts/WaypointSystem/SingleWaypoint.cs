using UnityEngine;
using FMODUnity;

namespace _Scripts.WaypointSystem {
    public class SingleWaypoint : MonoBehaviour {
        [SerializeField] string waypointInteractionLayer = "WaypointInteraction";
        [SerializeField] EventReference fmodEvent;

        TrackWaypointManager _trackWaypointManager;
        MeshRenderer _meshRenderer;

        int _waypointTriggerLayer;
        bool _isTriggered;

        void Awake() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _waypointTriggerLayer = LayerMask.NameToLayer(waypointInteractionLayer);
            //Hide();
        }

        public void SetTrackWaypoints(TrackWaypointManager trackWaypointManager) {
            _trackWaypointManager = trackWaypointManager;
        }

        void OnTriggerEnter(Collider other) {
            if (!other.gameObject.layer.Equals(_waypointTriggerLayer)) return;
            if (_isTriggered) return;
            _isTriggered = true;
            _trackWaypointManager.CarThroughWaypoint(this, other.transform);

            // Play FMOD event as a one-shot
            RuntimeManager.PlayOneShot(fmodEvent, transform.position);
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
            ResetTrigger();
        }

        public void ResetTrigger() {
            _isTriggered = false;
        }
    }
}
