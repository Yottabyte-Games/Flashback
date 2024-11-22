using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelCollider[] wheels;
    [Header("Settings")]
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float minSteeringAngle;
    [SerializeField] private float motorForce;
    [SerializeField] float brakeForce;
    [SerializeField] private float motorBrake;
    [SerializeField] private float maxSpeed;

    [Header("Debuging")]
    [SerializeField] private float speed;
    [SerializeField] private float currentBrakeforce;
    [SerializeField] private float verticalInput;

    private float horizontalInput;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Car speed to KM/H
        speed = rb.linearVelocity.magnitude * 3.6f;

        GetInput();
        HandleMotor();
        HandleSteering();
        ApplyBraking();
    }

    private void HandleMotor()
    {
        if (speed >= maxSpeed)
        {
            verticalInput = 0;
        }
        Debug.Log("AddingSpeed" + verticalInput);
        wheels[0].motorTorque = verticalInput * motorForce * Time.deltaTime;
        wheels[1].motorTorque = verticalInput * motorForce * Time.deltaTime;
        wheels[2].motorTorque = verticalInput * motorForce * Time.deltaTime;
        wheels[3].motorTorque = verticalInput * motorForce * Time.deltaTime;
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
        if (verticalInput < 0.1f)
        {
            motorBrakeForce = 0;
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