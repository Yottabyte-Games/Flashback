using Imp_Assets.GinjaGaming.FinalCharacterController.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;
using FMODUnity;

namespace Imp_Assets.GinjaGaming.FinalCharacterController.Scripts
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        #region Class Variables
        [Header("Components")]
        [SerializeField]
        CharacterController _characterController;
        [SerializeField] Camera _playerCamera;
        public float RotationMismatch { get; private set; }
        public bool IsRotatingToTarget { get; private set; }

        [Header("Base Movement")]
        public float walkAcceleration = 25f;
        public float walkSpeed = 2f;
        public float runAcceleration = 35f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;
        public float inAirAcceleration = 25f;
        public float drag = 20f;
        public float inAirDrag = 5f;
        public float gravity = 25f;
        public float terminalVelocity = 50f;
        public float jumpSpeed = 0.8f;
        public float movingThreshold = 0.01f;

        [Header("Animation")]
        public float playerModelRotationSpeed = 10f;
        public float rotateToTargetTime = 0.67f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;
        public bool CameraMovement { get; private set; } = true;

        [Header("Environment Details")]
        [SerializeField]
        LayerMask _groundLayers;

        PlayerLocomotionInput _playerLocomotionInput;
        PlayerState _playerState;

        Vector2 _cameraRotation = Vector2.zero;
        Vector2 _playerTargetRotation = Vector2.zero;

        bool _jumpedLastFrame;
        bool _isRotatingClockwise;
        float _rotatingToTargetTimer;
        float _verticalVelocity;
        float _antiBump;
        float _stepOffset;

        [Header("Audio")] EventInstance PlayerFootsteps;

        PlayerMovementState _lastMovementState = PlayerMovementState.Falling;
        #endregion

        #region Startup

        void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();

            _antiBump = sprintSpeed;
            _stepOffset = _characterController.stepOffset;
        }

        void Start()
        {
            PlayerFootsteps = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.PlayerFootsteps);
        }
        #endregion

        #region Update Logic

        void Update()
        {
            UpdateMovementState();
            

            HandleVerticalMovement();
            HandleLateralMovement();
            
        }

        void FixedUpdate()
        {
            UpdateSound();
        }

        void UpdateMovementState()
        {
            _lastMovementState = _playerState.CurrentPlayerMovementState;

            var canRun = CanRun();
            var isMovementInput = _playerLocomotionInput.MovementInput != Vector2.zero;             //order
            var isMovingLaterally = IsMovingLaterally();                                            //matters
            var isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;          //order
            var isWalking = isMovingLaterally && (!canRun || _playerLocomotionInput.WalkToggledOn); //matters
            var isGrounded = IsGrounded();

            var lateralState = isWalking ? PlayerMovementState.Walking :
                                               isSprinting ? PlayerMovementState.Sprinting :
                                               isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;

            _playerState.SetPlayerMovementState(lateralState);

            // Control Airborn State
            if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y > 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y <= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else
            {
                _characterController.stepOffset = _stepOffset;
            }
        }

        void HandleVerticalMovement()
        {
            var isGrounded = _playerState.InGroundedState();

            _verticalVelocity -= gravity * Time.deltaTime;

            if (isGrounded && _verticalVelocity < 0)
                _verticalVelocity = -_antiBump;

            if (_playerLocomotionInput.JumpPressed && isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(jumpSpeed * 3 * gravity);
                _jumpedLastFrame = true;
            }

            if (_playerState.IsStateGroundedState(_lastMovementState) && !isGrounded)
            {
                _verticalVelocity += _antiBump;
            }

            // Clamp at terminal velocity
            if (Mathf.Abs(_verticalVelocity) > Mathf.Abs(terminalVelocity))
            {
                _verticalVelocity = -1f * Mathf.Abs(terminalVelocity);
            }
        }

        void HandleLateralMovement()
        {
            // Create quick references for current state
            var isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            var isGrounded = _playerState.InGroundedState();
            var isWalking = _playerState.CurrentPlayerMovementState == PlayerMovementState.Walking;

            // State dependent acceleration and speed
            var lateralAcceleration = !isGrounded ? inAirAcceleration :
                                        isWalking ? walkAcceleration :
                                        isSprinting ? sprintAcceleration : runAcceleration;

            var clampLateralMagnitude = !isGrounded ? sprintSpeed :
                                          isWalking ? walkSpeed :
                                          isSprinting ? sprintSpeed : runSpeed;

            var cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            var cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            var movementDirection = cameraRightXZ * _playerLocomotionInput.MovementInput.x + cameraForwardXZ * _playerLocomotionInput.MovementInput.y;

            var movementDelta = lateralAcceleration * Time.deltaTime * movementDirection;
            var newVelocity = _characterController.velocity + movementDelta;

            // Add drag to player
            var dragMagnitude = isGrounded ? drag : inAirDrag;
            var currentDrag = dragMagnitude * Time.deltaTime * newVelocity.normalized;
            newVelocity = (newVelocity.magnitude > dragMagnitude * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), clampLateralMagnitude);
            newVelocity.y += _verticalVelocity;
            newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;

            // Move character (Unity suggests only calling this once per tick)
            _characterController.Move(newVelocity * Time.deltaTime);
        }

        Vector3 HandleSteepWalls(Vector3 velocity)
        {
            var normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
            var angle = Vector3.Angle(normal, Vector3.up);
            var validAngle = angle <= _characterController.slopeLimit;

            if (!validAngle && _verticalVelocity < 0f)
                velocity = Vector3.ProjectOnPlane(velocity, normal);

            return velocity;
        }

        void UpdateSound()
        {
            // Check if the player is moving laterally
            bool isMovingLaterally = IsMovingLaterally();

            // Check if the player is grounded
            bool isGrounded = IsGrounded();

            // Start or stop the footstep event based on movement and grounded state
            if (isMovingLaterally && isGrounded)
            {
                PLAYBACK_STATE playbackState;
                PlayerFootsteps.getPlaybackState(out playbackState);
                if (playbackState != PLAYBACK_STATE.PLAYING)
                {
                    PlayerFootsteps.start();
                }
            }
            else
            {
                // Stop the FMOD footstep event
                if (PlayerFootsteps.isValid())
                {
                    PlayerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                }
            }
        }
        #endregion

        #region Late Update Logic

        void LateUpdate()
        {
            if(CameraMovement)
                UpdateCameraRotation();
        }

        void UpdateCameraRotation()
        {
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;

            var rotationTolerance = 90f;
            var isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            IsRotatingToTarget = _rotatingToTargetTimer > 0;

            // ROTATE if we're not idling
            if (!isIdling)
            {
                RotatePlayerToTarget();
            }
            // If rotation mismatch not within tolerance, or rotate to target is active, ROTATE
            else if (Mathf.Abs(RotationMismatch) > rotationTolerance || IsRotatingToTarget)
            {
                UpdateIdleRotation(rotationTolerance);
            }

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);

            // Get angle between camera and player
            var camForwardProjectedXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            var crossProduct = Vector3.Cross(transform.forward, camForwardProjectedXZ);
            var sign = Mathf.Sign(Vector3.Dot(crossProduct, transform.up));
            RotationMismatch = sign * Vector3.Angle(transform.forward, camForwardProjectedXZ);
        }

        void UpdateIdleRotation(float rotationTolerance)
        {
            // Initiate new rotation direction
            if (Mathf.Abs(RotationMismatch) > rotationTolerance)
            {
                _rotatingToTargetTimer = rotateToTargetTime;
                _isRotatingClockwise = RotationMismatch > rotationTolerance;
            }
            _rotatingToTargetTimer -= Time.deltaTime;

            // Rotate player
            if (_isRotatingClockwise && RotationMismatch > 0f ||
                !_isRotatingClockwise && RotationMismatch < 0f)
            {
                RotatePlayerToTarget();
            }
        }

        void RotatePlayerToTarget()
        {
            var targetRotationX = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, playerModelRotationSpeed * Time.deltaTime);
        }

        public void ToggleCameraMovement(bool toggle)
        {
            CameraMovement = toggle;
        }
        #endregion

        #region State Checks

        bool IsMovingLaterally()
        {
            var lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreshold;
        }

        bool IsGrounded()
        {
            var grounded = _playerState.InGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirborne();

            return grounded;
        }

        bool IsGroundedWhileGrounded()
        {
            var spherePosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);

            var grounded = Physics.CheckSphere(spherePosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);

            return grounded;
        }

        bool IsGroundedWhileAirborne()
        {
            var normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
            var angle = Vector3.Angle(normal, Vector3.up);
            var validAngle = angle <= _characterController.slopeLimit;

            return _characterController.isGrounded && validAngle;
        }

        bool CanRun()
        {
            // This means player is moving diagonally at 45 degrees or forward, if so, we can run
            return _playerLocomotionInput.MovementInput.y >= Mathf.Abs(_playerLocomotionInput.MovementInput.x);
        }
        #endregion
    }
}
