using System;
using System.Linq;
using GitAmendKartSample.Scripts.Utils.Extensions;
using Unity.Cinemachine;
using UnityEngine;

namespace GitAmendKartSample.Scripts {
    [Serializable]
    public class AxleInfo {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor;
        public bool steering;
        public WheelFrictionCurve OriginalForwardFriction;
        public WheelFrictionCurve OriginalSidewaysFriction;
    }

    public class KartController : MonoBehaviour {
        [Header("Axle Information")]
        [SerializeField] AxleInfo[] axleInfos;

        [Header("Motor Attributes")]
        [SerializeField] float maxMotorTorque = 3000f;
        [SerializeField] float maxSpeed;

        [Header("Steering Attributes")]
        [SerializeField] float maxSteeringAngle = 30f;
        [SerializeField] AnimationCurve turnCurve;
        [SerializeField] float turnStrength = 1500f;

        [Header("Braking and Drifting")]
        [SerializeField] float driftSteerMultiplier = 1.5f; // Change in steering during a drift
        [SerializeField] float brakeTorque = 10000f;

        [Header("Physics")]
        [SerializeField] Transform centerOfMass;
        [SerializeField] float downForce = 100f;
        [SerializeField] float gravity = Physics.gravity.y;
        [SerializeField] float lateralGScale = 10f; // Scaling factor for lateral G forces;

        [Header("Banking")]
        [SerializeField] float maxBankAngle = 5f;
        [SerializeField] float bankSpeed = 2f;

        [Header("Refs")]
        [SerializeField] InputReader playerInput;
        [SerializeField] Circuit circuit;
        [SerializeField] AIDriverData driverData;
        [SerializeField] CinemachineVirtualCamera playerCamera;
        [SerializeField] AudioListener playerAudioListener;

        IDrive _input;
        Rigidbody _rb;

        Vector3 _kartVelocity;
        float _brakeVelocity;
        float _driftVelocity;

        float _originalY;
        float _adjustedY;
        float _yDiff;
        Vector3 _syncPosition;

        RaycastHit _hit;

        const float ThresholdSpeed = 10f;
        const float CenterOfMassOffset = -0.5f;
        Vector3 _originalCenterOfMass;

        public bool IsGrounded = true;
        public Vector3 Velocity => _kartVelocity;
        public float MaxSpeed => maxSpeed;

        void Awake() {
            
            if (playerInput is IDrive driveInput) {
                _input = driveInput;
            }
            
            _rb = GetComponent<Rigidbody>();
            _input.Enable();

            _rb.centerOfMass = centerOfMass.localPosition;
            _originalCenterOfMass = centerOfMass.localPosition;

            foreach (AxleInfo axleInfo in axleInfos) {
                axleInfo.OriginalForwardFriction = axleInfo.leftWheel.forwardFriction;
                axleInfo.OriginalSidewaysFriction = axleInfo.leftWheel.sidewaysFriction;
            }
        }

        public void SetInput(IDrive input) {
            this._input = input;
            if (_input != null) {
                _input.Enable();
            } else {
                Debug.LogWarning("Assigned input does not implement IDrive.");
            }
        }

        void Start() {
            playerCamera.Priority = 100;
            playerAudioListener.enabled = true;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                transform.position += transform.forward * 20f;
            }
        }

        void Move(Vector2 inputVector) {
            float verticalInput = AdjustInput(_input.Move.y);
            float horizontalInput = AdjustInput(_input.Move.x);

            float motor = maxMotorTorque * verticalInput;
            float steering = maxSteeringAngle * horizontalInput;

            UpdateAxles(motor, steering);
            UpdateBanking(horizontalInput);

            _kartVelocity = transform.InverseTransformDirection(_rb.linearVelocity);

            if (IsGrounded) {
                HandleGroundedMovement(verticalInput, horizontalInput);
            } else {
                HandleAirborneMovement(verticalInput, horizontalInput);
            }
        }

        void HandleGroundedMovement(float verticalInput, float horizontalInput) {
            // Turn logic
            if (Mathf.Abs(verticalInput) > 0.1f || Mathf.Abs(_kartVelocity.z) > 1) {
                float turnMultiplier = Mathf.Clamp01(turnCurve.Evaluate(_kartVelocity.magnitude / maxSpeed));
                _rb.AddTorque(Vector3.up * (horizontalInput * Mathf.Sign(_kartVelocity.z) * turnStrength * 100f * turnMultiplier));
            }

            // Acceleration Logic
            if (!_input.IsBraking) {
                float targetSpeed = verticalInput * maxSpeed;
                Vector3 forwardWithoutY = transform.forward.With(y: 0).normalized;
                _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, forwardWithoutY * targetSpeed, Time.deltaTime);
            }

            // Downforce - always push the cart down, using lateral Gs to scale the force if the Kart is moving sideways fast
            float speedFactor = Mathf.Clamp01(_rb.linearVelocity.magnitude / maxSpeed);
            float lateralG = Mathf.Abs(Vector3.Dot(_rb.linearVelocity, transform.right));
            float downForceFactor = Mathf.Max(speedFactor, lateralG / lateralGScale);
            _rb.AddForce(-transform.up * (downForce * _rb.mass * downForceFactor));

            // Shift Center of Mass
            float speed = _rb.linearVelocity.magnitude;
            Vector3 centerOfMassAdjustment = (speed > ThresholdSpeed)
                ? new Vector3(0f, 0f, Mathf.Abs(verticalInput) > 0.1f ? Mathf.Sign(verticalInput) * CenterOfMassOffset : 0f)
                : Vector3.zero;
            _rb.centerOfMass = _originalCenterOfMass + centerOfMassAdjustment;
        }

        void HandleAirborneMovement(float verticalInput, float horizontalInput) {
            // Apply gravity to the Kart while its airborne
            _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, _rb.linearVelocity + Vector3.down * gravity, Time.deltaTime * gravity);
        }

        void UpdateBanking(float horizontalInput) {
            // Bank the Kart in the opposite direction of the turn
            float targetBankAngle = horizontalInput * -maxBankAngle;
            Vector3 currentEuler = transform.localEulerAngles;
            currentEuler.z = Mathf.LerpAngle(currentEuler.z, targetBankAngle, Time.deltaTime * bankSpeed);
            transform.localEulerAngles = currentEuler;
        }

        void UpdateAxles(float motor, float steering) {
            foreach (AxleInfo axleInfo in axleInfos) {
                HandleSteering(axleInfo, steering);
                HandleMotor(axleInfo, motor);
                HandleBrakesAndDrift(axleInfo);
                UpdateWheelVisuals(axleInfo.leftWheel);
                UpdateWheelVisuals(axleInfo.rightWheel);
            }
        }

        void UpdateWheelVisuals(WheelCollider collider) {
            if (collider.transform.childCount == 0) return;

            Transform visualWheel = collider.transform.GetChild(0);

            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);

            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }

        void HandleSteering(AxleInfo axleInfo, float steering) {
            if (axleInfo.steering) {
                float steeringMultiplier = _input.IsBraking ? driftSteerMultiplier : 1f;
                axleInfo.leftWheel.steerAngle = steering * steeringMultiplier;
                axleInfo.rightWheel.steerAngle = steering * steeringMultiplier;
            }
        }

        void HandleMotor(AxleInfo axleInfo, float motor) {
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
        }

        void HandleBrakesAndDrift(AxleInfo axleInfo)
        {
            if (!axleInfo.motor) return;
            if (_input.IsBraking) {
                _rb.constraints = RigidbodyConstraints.FreezeRotationX;

                float newZ = Mathf.SmoothDamp(_rb.linearVelocity.z, 0, ref _brakeVelocity, 1f);
                _rb.linearVelocity = _rb.linearVelocity.With(z: newZ);

                axleInfo.leftWheel.brakeTorque = brakeTorque;
                axleInfo.rightWheel.brakeTorque = brakeTorque;
                ApplyDriftFriction(axleInfo.leftWheel);
                ApplyDriftFriction(axleInfo.rightWheel);
            } else {
                _rb.constraints = RigidbodyConstraints.None;

                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
                ResetDriftFriction(axleInfo.leftWheel);
                ResetDriftFriction(axleInfo.rightWheel);
            }
        }

        void ResetDriftFriction(WheelCollider wheel) {
            AxleInfo axleInfo = axleInfos.FirstOrDefault(axle => Equals(axle.leftWheel, wheel) || Equals(axle.rightWheel, wheel));
            if (axleInfo == null) return;

            wheel.forwardFriction = axleInfo.OriginalForwardFriction;
            wheel.sidewaysFriction = axleInfo.OriginalSidewaysFriction;
        }

        void ApplyDriftFriction(WheelCollider wheel) {
            if (wheel.GetGroundHit(out var hit)) {
                wheel.forwardFriction = UpdateFriction(wheel.forwardFriction);
                wheel.sidewaysFriction = UpdateFriction(wheel.sidewaysFriction);
                IsGrounded = true;
            }
        }

        WheelFrictionCurve UpdateFriction(WheelFrictionCurve friction) {
            friction.stiffness = _input.IsBraking ? Mathf.SmoothDamp(friction.stiffness, .5f, ref _driftVelocity, Time.deltaTime * 2f) : 1f;
            return friction;
        }

        float AdjustInput(float input) {
            return input switch {
                >= .7f => 1f,
                <= -.7f => -1f,
                _ => input
            };
        }
    }
}