using UnityEngine;

namespace YottabyteGames.FinalCharacterController.Scripts
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] Animator _animator;
        [SerializeField] float locomotionBlendSpeed = 0.02f;

        PlayerLocomotionInput _playerLocomotionInput;
        PlayerState _playerState;

        static int inputXHash = Animator.StringToHash("inputX");
        static int inputYHash = Animator.StringToHash("inputY");
        static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
        static int isGroundedHash = Animator.StringToHash("isGrounded");
        static int isFallingHash = Animator.StringToHash("isFalling");
        static int isJumpingHash = Animator.StringToHash("isJumping");

        Vector3 _currentBlendInput = Vector3.zero;

        void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }

        void Update()
        {
            UpdateAnimationState();
        }

        void UpdateAnimationState()
        {
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            bool isRunning = _playerState.CurrentPlayerMovementState == PlayerMovementState.Running;
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isJumping = _playerState.CurrentPlayerMovementState == PlayerMovementState.Jumping;
            bool isFalling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Falling;
            bool isGrounded = _playerState.InGroundState();

            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * 1.5f : _playerLocomotionInput.MovementInput;
            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);


            _animator.SetBool(isGroundedHash, isGrounded);
            _animator.SetBool(isFallingHash, isFalling);
            _animator.SetBool(isJumpingHash, isJumping);
            _animator.SetFloat(inputXHash, inputTarget.x);
            _animator.SetFloat(inputYHash, inputTarget.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);
        }
    }
}
