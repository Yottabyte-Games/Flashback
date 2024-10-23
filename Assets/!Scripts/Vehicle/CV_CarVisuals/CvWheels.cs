using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CV_CarVisuals
{
    public class CvWheels : MonoBehaviour
    {
        Rigidbody _carRb;
        Transform _attachPoint;
        PlayerInputs _input;
        VehicleSpeed _vehicleSpeed;
        public bool grounded;

        public LayerMask groundLayer;

        [Header("Wheel Dimensions")]
        public WheelPosition wheelPosition;
        public float wheelRadius;
        public float suspensionMaxHeight;
        public float frontWheelTurnMaxAngle = 30;

        [Header("Effects")]
        TrailRenderer _tyreTrail;
        ParticleSystem _dustParticles;
        bool _isDustParticlesNull;
        bool _isTyreTrailNull;

        void Awake()
        {
            _attachPoint = transform.parent;

            _tyreTrail = GetComponentInChildren<TrailRenderer>();
            _dustParticles = GetComponentInChildren<ParticleSystem>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _isTyreTrailNull = _tyreTrail == null;
            _isDustParticlesNull = _dustParticles == null;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetUpWheel(Rigidbody carRb)
        {
            _carRb = carRb;
            wheelPosition = transform.parent.localPosition.z > 0 ? WheelPosition.Front : WheelPosition.Back;
        }

        public void ProcessWheelVisuals(PlayerInputs input, VehicleSpeed vehicleSpeed)
        {
            _input = input;
            _vehicleSpeed = vehicleSpeed;
            UpdateWheelPosition();
            UpdateWheelRotations();
            UpdateWheelParticles();
            UpdateWheelTrails();
        }

        void UpdateWheelPosition()
        {
            RaycastHit hit;

            grounded = Physics.Raycast(_attachPoint.position, -_attachPoint.up, out hit, suspensionMaxHeight);

            var wheelExtension = suspensionMaxHeight;

            if (grounded)
            {
                wheelExtension = hit.distance;
            }

            transform.position = _attachPoint.position + ((wheelExtension - wheelRadius) * -_attachPoint.up);
        }

        void UpdateWheelRotations()
        {
            //In case tyre model is flipped
            var rotationDirection = transform.parent.localScale.x > 0 ? 1 : -1;

            //Front wheel steering - Checks if the wheel is in front of the cars center of mass
            if (wheelPosition == WheelPosition.Front)
                transform.localRotation = Quaternion.Euler(0, (_input.steeringInput * frontWheelTurnMaxAngle * rotationDirection), 0);

            //Converts linear speed into angular speed and applies it to the wheels
            var wheelCircumference = 2 * Mathf.PI * (wheelRadius / 2);
            var angularVelocity = _vehicleSpeed.forwardSpeed / wheelCircumference /** Time.deltaTime*/;
            //transform.GetChild(0).Rotate(0, angularVelocity, 0);
            transform.GetChild(0).Rotate(angularVelocity, 0, 0);
        }

        void UpdateWheelParticles()
        {
            if (_isDustParticlesNull)
                return;

            var isBackWheel = (wheelPosition == WheelPosition.Back);
            var isGoingFastEnough = (_vehicleSpeed.forwardSpeedPercent > 0.5f && _vehicleSpeed.forwardSpeed > _vehicleSpeed.sideSpeed);

            if (isBackWheel &&
                isGoingFastEnough &&
                grounded)
            {
                if (!_dustParticles.isPlaying)
                {
                    _dustParticles.Play();
                }
                return;
            }

            _dustParticles.Stop();
        }

        void UpdateWheelTrails()
        {
            if (_isTyreTrailNull)
                return;

            var overSideSpeedThreshold = false;

            switch (wheelPosition)
            {
                case WheelPosition.Front:
                    overSideSpeedThreshold = Mathf.Abs(_vehicleSpeed.sideSpeed) > Mathf.Abs(_vehicleSpeed.forwardSpeed / 2);
                    break;
                case WheelPosition.Back:
                    overSideSpeedThreshold = Mathf.Abs(_vehicleSpeed.sideSpeed) > 5 /*Mathf.Abs(_vehicleSpeed.ForwardSpeed / 2)*/;
                    break;
            }

            _tyreTrail.transform.position = transform.position + wheelRadius * -_attachPoint.up;
            _tyreTrail.emitting = overSideSpeedThreshold && _vehicleSpeed.forwardSpeed > 1 && grounded;
        }

    }

    public enum WheelPosition { Front, Back, Other }
}