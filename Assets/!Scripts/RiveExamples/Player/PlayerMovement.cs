using UnityEditor;
using UnityEngine;

namespace _Scripts.RiveExamples.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] public float moveSpeed;

        public float groundDrag;

        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplier;
        bool _readyToJump = true;

        [Header("Keybinds")] public KeyCode jumpKey = KeyCode.Space;

        [Header("Ground Check")] public float playerHeight;
        public LayerMask whatIsGround;
        bool _grounded;

        public Transform orientation;

        float _horizontalInput;
        float _verticalInput;

        Vector3 _moveDirection;
        Rigidbody _rb;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }

            // ground check
            _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

            HandleInput();
            SpeedControl();

            // handle drag
            if (_grounded)
            {
                _rb.linearDamping = groundDrag;
            }
            else
            {
                _rb.linearDamping = 0;
            }
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        void HandleInput()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(jumpKey) && _readyToJump && _grounded)
            {
                _readyToJump = false;
                Jump();
                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

        void MovePlayer()
        {
            // calculate movement direction
            _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

            if (_grounded)
            {
                _rb.AddForce(moveSpeed * 10f * _moveDirection.normalized, ForceMode.Force);
            }
            else
            {
                _rb.AddForce(moveSpeed * airMultiplier * 10f * _moveDirection.normalized, ForceMode.Force);
            }
        }

        void SpeedControl()
        {
            var velocity = _rb.linearVelocity;
            Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
            }
        }

        void Jump()
        {
            // reset y velocity
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        void ResetJump()
        {
            _readyToJump = true;
        }
    }
}
