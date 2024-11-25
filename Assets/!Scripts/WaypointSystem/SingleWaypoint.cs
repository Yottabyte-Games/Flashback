using UnityEngine;
using FMODUnity;

namespace _Scripts.WaypointSystem {
    public class SingleWaypoint : MonoBehaviour {
        [SerializeField] string waypointInteractionLayer = "WaypointInteraction";
        [SerializeField] EventReference fmodEvent;

        TrackWaypoints _trackWaypoints;
        MeshRenderer _meshRenderer;

        int _waypointTriggerLayer;
        bool _isTriggered;

        void Awake() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _waypointTriggerLayer = LayerMask.NameToLayer(waypointInteractionLayer);
            Hide();
        }

        public void SetTrackWaypoints(TrackWaypoints trackWaypoints) {
            _trackWaypoints = trackWaypoints;
        }

        void OnTriggerEnter(Collider other) {
            if (!other.gameObject.layer.Equals(_waypointTriggerLayer)) return;
            if (_isTriggered) return;
            _isTriggered = true;
            _trackWaypoints.CarThroughWaypoint(this, other.transform);

            // Play FMOD event as a one-shot
            RuntimeManager.PlayOneShot(fmodEvent, transform.position);
        }

        public void Show() {
            if (_meshRenderer is not null) _meshRenderer.enabled = true;
        }

        public void Hide() {
            if (_meshRenderer is not null) _meshRenderer.enabled = false;
            ResetTrigger();
        }

        public void ResetTrigger() {
            _isTriggered = false;
        }
    }
}
