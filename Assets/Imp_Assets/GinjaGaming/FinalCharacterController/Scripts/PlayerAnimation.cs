using System.Linq;
using Imp_Assets.GinjaGaming.FinalCharacterController.Scripts.Input;
using UnityEngine;

namespace Imp_Assets.GinjaGaming.FinalCharacterController.Scripts
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] float locomotionBlendSpeed = 4f;

        PlayerLocomotionInput _playerLocomotionInput;
        PlayerState _playerState;
        PlayerController _playerController;
        PlayerActionsInput _playerActionsInput;

        // Locomotion
        static int inputXHash = Animator.StringToHash("inputX");
        static int inputYHash = Animator.StringToHash("inputY");
        static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
        static int isIdlingHash = Animator.StringToHash("isIdling");
        static int isGroundedHash = Animator.StringToHash("isGrounded");
        static int isFallingHash = Animator.StringToHash("isFalling");
        static int isJumpingHash = Animator.StringToHash("isJumping");

        // Actions
        static int isAttackingHash = Animator.StringToHash("isAttacking");
        static int isGatheringHash = Animator.StringToHash("isGathering");
        static int isPlayingActionHash = Animator.StringToHash("isPlayingAction");
        int[] actionHashes;

        // Camera/Rotation
        static int isRotatingToTargetHash = Animator.StringToHash("isRotatingToTarget");
        static int rotationMismatchHash = Animator.StringToHash("rotationMismatch");

        Vector3 _currentBlendInput = Vector3.zero;

        float _sprintMaxBlendValue = 1.5f;
        float _runMaxBlendValue = 1.0f;
        float _walkMaxBlendValue = 0.5f;

        void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
            _playerController = GetComponent<PlayerController>();
            _playerActionsInput = GetComponent<PlayerActionsInput>();

            actionHashes = new int[] { isGatheringHash };
        }

        void Update()
        {
            UpdateAnimationState();
        }

        void UpdateAnimationState()
        {
            var isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            var isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
            var isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            var isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
            var isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
            var isGrounded = _playerState.InGroundedState();
            var isPlayingAction = actionHashes.Any(hash => _animator.GetBool(hash));

            var isRunBlendValue = isRunning || isJumping || isFalling;

            var inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * _sprintMaxBlendValue :
                                  isRunBlendValue ? _playerLocomotionInput.MovementInput * _runMaxBlendValue : 
                                                    _playerLocomotionInput.MovementInput * _walkMaxBlendValue;

            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

            _animator.SetBool(isGroundedHash, isGrounded);
            _animator.SetBool(isIdlingHash, isIdling);
            _animator.SetBool(isFallingHash, isFalling);
            _animator.SetBool(isJumpingHash, isJumping);
            _animator.SetBool(isRotatingToTargetHash, _playerController.IsRotatingToTarget);
            _animator.SetBool(isAttackingHash, _playerActionsInput.AttackPressed);
            _animator.SetBool(isGatheringHash, _playerActionsInput.GatherPressed);
            _animator.SetBool(isPlayingActionHash, isPlayingAction);

            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
            _animator.SetFloat(rotationMismatchHash, _playerController.RotationMismatch);
        }
    }
}
