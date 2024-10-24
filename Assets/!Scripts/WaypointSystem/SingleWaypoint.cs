using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.WaypointSystem
{
    public class SingleWaypoint : MonoBehaviour {
        [SerializeField] string waypointInteractionLayer = "WaypointInteraction";
        int _waypointTriggerLayer;
        bool _isTriggered;

        TrackWaypoints _trackWaypoints;
        MeshRenderer _meshRenderer;

        void Awake() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _waypointTriggerLayer = LayerMask.NameToLayer(waypointInteractionLayer);
            Hide();
        }

        public void SetTrackWaypoints(TrackWaypoints trackWaypoints) {
            _trackWaypoints = trackWaypoints;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != _waypointTriggerLayer || _isTriggered) return;
            _isTriggered = true;
            _trackWaypoints.CarThroughWaypoint(this, other.transform);
        }

        public void Show()
        {
            if (_meshRenderer is null) return;
            _meshRenderer.enabled = true;
        }

        public void Hide()
        {
            if (_meshRenderer is null) return;
            _meshRenderer.enabled = false;
            ResetTrigger();
        }

        public void ResetTrigger()
        {
            _isTriggered = false; // Reset the trigger flag
        }
    }
}