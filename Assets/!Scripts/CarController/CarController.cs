using UnityEngine;
namespace _Scripts.CarController
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] WheelCollider[] wheels;
        [Header("Settings")]
        [SerializeField] float maxSteeringAngle;
        [SerializeField] float minSteeringAngle;
        [SerializeField] float motorForce;
        [SerializeField] float brakeForce;
        [SerializeField] float motorBrake;
        [SerializeField] float maxSpeed;

        [Header("Debuging")]
        [SerializeField] float speed;
        [SerializeField] float currentBrakeforce;
        [SerializeField] float verticalInput;
        [SerializeField] float rpm;

        float horizontalInput;
        Rigidbody rb;
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            rpm = wheels[0].rpm;
        
            // Car speed to KM/H
            speed = rb.linearVelocity.magnitude * 3.6f;

            GetInput();
            HandleMotor();
            HandleSteering();
            ApplyBraking();
        }

        void HandleMotor()
        {
            if (speed >= maxSpeed)
            {
                verticalInput = 0;
            }
            /*
        if (verticalInput == -1)
        {
            verticalInput *= 100;
        }*/
            Debug.Log("AddingSpeed" + verticalInput);
            wheels[0].motorTorque = verticalInput * motorForce;
            wheels[1].motorTorque = verticalInput * motorForce;
            wheels[2].motorTorque = verticalInput * motorForce;
            wheels[3].motorTorque = verticalInput * motorForce;
        }

        void ApplyBraking()
        {
            //If braking, apply effect to all wheels
            if (Input.GetKey(KeyCode.Space))
            {
                currentBrakeforce = brakeForce;
            }
            else
            {
                currentBrakeforce = ApplyMotorBrake();
            }
            foreach (WheelCollider wheel in wheels)
            {
                wheel.brakeTorque = currentBrakeforce;
            }
        }

        float ApplyMotorBrake()
        {
            float motorBrakeForce;
            if (verticalInput == 0)
            {
                motorBrakeForce = motorBrake;
            }
            else
            {
                motorBrakeForce = 0;
            }
            return motorBrakeForce;
        }


        void HandleSteering()
        {
            //Adjust steering angle to speed
            float steeringAngle = Mathf.Lerp(maxSteeringAngle, minSteeringAngle, speed/maxSpeed);

            //Applies steering to frontwheels
            wheels[0].steerAngle = horizontalInput * steeringAngle;
            wheels[1].steerAngle = horizontalInput * steeringAngle;
        }


        void GetInput()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }


    }
}