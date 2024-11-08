using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float movementSpeed;
    public float jumpSpeed;
    public float runMultiplier;
    public float gravity = -9.81f;    
    Vector3 velocity;
    CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
            if(characterController.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");

            var movement = transform.right * x + transform.forward * z;

            characterController.Move(movement * movementSpeed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            characterController.Move(velocity * Time.deltaTime);

            if(Input.GetButton("Jump") && characterController.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpSpeed * -2f * gravity);
            }

            if(Input.GetKey(KeyCode.LeftShift))
            {
                characterController.Move(movement * Time.deltaTime * runMultiplier);
            }

        }
}
