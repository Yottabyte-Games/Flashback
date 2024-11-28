/*
MESSAGE FROM CREATOR: This script was coded by Mena. You can use it in your games either these are commercial or
personal projects. You can even add or remove functions as you wish. However, you cannot sell copies of this
script by itself, since it is originally distributed as a free product.
I wish you the best for your project. Good luck!

P.S: If you need more cars, you can check my other vehicle assets on the Unity Asset Store, perhaps you could find
something useful for your game. Best regards, Mena.
*/

using System;
using _Scripts.Audio;
using _Scripts.WaypointSystem;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;
namespace Imp_Assets.PROMETEO___Car_Controller.Scripts
{
  public class PrometeoCarController : MonoBehaviour
  {
    [SerializeField] CarRespawnOnWaypoint carRespawnOnWaypoint;
    
    [SerializeField] FMODEventsSO fmodEventsSo;

    public EventReference Dialogue => fmodEventsSo.Dialogue;
    
    [Range(20, 300)]
    public int MaxSpeed = 90; //The maximum speed that the car can reach in km/h.
    [Range(10, 120)]
    public int MaxReverseSpeed = 45; //The maximum speed that the car can reach while going on reverse in km/h.
    [Range(1, 100)]
    public int AccelerationMultiplier = 2; // How fast the car can accelerate. 1 is a slow acceleration and 10 is the fastest.
    [Space(10)]
    [Range(10, 45)]
    public int MaxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
    [Range(0.1f, 1f)]
    public float SteeringSpeed = 0.5f; // How fast the steering wheel turns.
            
    [Range(100, 600)]
    public int BrakeForce = 350; // The strength of the wheel brakes.
    [Range(1, 10)]
    public int DecelerationMultiplier = 2; // How fast the car decelerates when the user is not using the throttle.
    [Range(1, 10)]
    public int HandbrakeDriftMultiplier = 5; // How much grip the car loses when the user hit the handbrake.
    [Space(10)]
    public Vector3 BodyMassCenter; // This is a vector that contains the center of mass of the car. I recommend to set this value
    // in the points x = 0 and z = 0 of your car. You can select the value that you want in the y axis,
    // however, you must notice that the higher this value is, the more unstable the car becomes.
    // Usually the y value goes from 0 to 1.5.


    /*
      The following variables are used to store the wheels' data of the car. We need both the mesh-only game objects and wheel
      collider components of the wheels. The wheel collider components and 3D meshes of the wheels cannot come from the same
      game object; they must be separate game objects.
      */
    public GameObject FrontLeftMesh;
    public WheelCollider FrontLeftCollider;
    [Space(10)]
    public GameObject FrontRightMesh;
    public WheelCollider FrontRightCollider;
    [Space(10)]
    public GameObject RearLeftMesh;
    public WheelCollider RearLeftCollider;
    [Space(10)]
    public GameObject RearRightMesh;
    public WheelCollider RearRightCollider;

    //PARTICLE SYSTEMS
    [Space(20)]
    //[Header("EFFECTS")]
    [Space(10)]
    //The following variable lets you to set up particle systems in your car
    public bool UseEffects = false;

    // The following particle systems are used as tire smoke when the car drifts.
    public ParticleSystem RlwParticleSystem;
    public ParticleSystem RrwParticleSystem;

    [Space(10)]
    // The following trail renderers are used as tire skids when the car loses traction.
    public TrailRenderer RlwTireSkid;
    public TrailRenderer RrwTireSkid;

    //SPEED TEXT (UI)
    [Space(20)]
    //[Header("UI")]
    [Space(10)]
    //The following variable lets you to set up a UI text to display the speed of your car.
    public bool UseUI = false;
    public Text CarSpeedText; // Used to store the UI object that is going to show the speed of the car.

    //SOUNDS
    [Space(20)]
    //[Header("Sounds")]
    [Space(10)]
    //The following variable lets you to set up sounds for your car such as the car engine or tire screech sounds.
    public bool UseSounds = false;
    public AudioSource CarEngineSound; // This variable stores the sound of the car engine.
    public AudioSource TireScreechSound; // This variable stores the sound of the tire screech (when the car is drifting).
    float _initialCarEngineSoundPitch; // Used to store the initial pitch of the car engine sound.

    //CONTROLS
    [Space(20)]
    //[Header("CONTROLS")]
    [Space(10)]
    //The following variables lets you to set up touch controls for mobile devices.
    public bool UseTouchControls = false;
    public GameObject ThrottleButton;
    PrometeoTouchInput _throttlePti;
    public GameObject ReverseButton;
    PrometeoTouchInput _reversePti;
    public GameObject TurnRightButton;
    PrometeoTouchInput _turnRightPti;
    public GameObject TurnLeftButton;
    PrometeoTouchInput _turnLeftPti;
    public GameObject HandbrakeButton;
    PrometeoTouchInput _handbrakePti;

    //CAR DATA

    [HideInInspector]
    public float CarSpeed; // Used to store the speed of the car.
    [HideInInspector]
    public bool IsDrifting; // Used to know whether the car is drifting or not.
    [HideInInspector]
    public bool IsTractionLocked; // Used to know whether the traction of the car is locked or not.

    //PRIVATE VARIABLES

    /*
      IMPORTANT: The following variables should not be modified manually since their values are automatically given via script.
      */
    Rigidbody _carRigidbody; // Stores the car's rigidbody.
    float _steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
    float _throttleAxis; // Used to know whether the throttle has reached the maximum value. It goes from -1 to 1.
    float _driftingAxis;
    float _localVelocityZ;
    float _localVelocityX;
    bool _deceleratingCar;
    bool _touchControlsSetup = false;
    /*
      The following variables are used to store information about sideways friction of the wheels (such as
      extremumSlip,extremumValue, asymptoteSlip, asymptoteValue and stiffness). We change this values to
      make the car to start drifting.
      */
    WheelFrictionCurve _fLwheelFriction;
    float _flWextremumSlip;
    WheelFrictionCurve _fRwheelFriction;
    float _frWextremumSlip;
    WheelFrictionCurve _rLwheelFriction;
    float _rlWextremumSlip;
    WheelFrictionCurve _rRwheelFriction;
    float _rrWextremumSlip;

    // Start is called before the first frame update
    void Start()
    {
      //In this part, we set the 'carRigidbody' value with the Rigidbody attached to this
      //gameObject. Also, we define the center of mass of the car with the Vector3 given
      //in the inspector.
      _carRigidbody = gameObject.GetComponent<Rigidbody>();
      _carRigidbody.centerOfMass = BodyMassCenter;

      //Initial setup to calculate the drift value of the car. This part could look a bit
      //complicated, but do not be afraid, the only thing we're doing here is to save the default
      //friction values of the car wheels so we can set an appropiate drifting value later.
      _fLwheelFriction = new WheelFrictionCurve ();
      _fLwheelFriction.extremumSlip = FrontLeftCollider.sidewaysFriction.extremumSlip;
      _flWextremumSlip = FrontLeftCollider.sidewaysFriction.extremumSlip;
      _fLwheelFriction.extremumValue = FrontLeftCollider.sidewaysFriction.extremumValue;
      _fLwheelFriction.asymptoteSlip = FrontLeftCollider.sidewaysFriction.asymptoteSlip;
      _fLwheelFriction.asymptoteValue = FrontLeftCollider.sidewaysFriction.asymptoteValue;
      _fLwheelFriction.stiffness = FrontLeftCollider.sidewaysFriction.stiffness;
      _fRwheelFriction = new WheelFrictionCurve ();
      _fRwheelFriction.extremumSlip = FrontRightCollider.sidewaysFriction.extremumSlip;
      _frWextremumSlip = FrontRightCollider.sidewaysFriction.extremumSlip;
      _fRwheelFriction.extremumValue = FrontRightCollider.sidewaysFriction.extremumValue;
      _fRwheelFriction.asymptoteSlip = FrontRightCollider.sidewaysFriction.asymptoteSlip;
      _fRwheelFriction.asymptoteValue = FrontRightCollider.sidewaysFriction.asymptoteValue;
      _fRwheelFriction.stiffness = FrontRightCollider.sidewaysFriction.stiffness;
      _rLwheelFriction = new WheelFrictionCurve ();
      _rLwheelFriction.extremumSlip = RearLeftCollider.sidewaysFriction.extremumSlip;
      _rlWextremumSlip = RearLeftCollider.sidewaysFriction.extremumSlip;
      _rLwheelFriction.extremumValue = RearLeftCollider.sidewaysFriction.extremumValue;
      _rLwheelFriction.asymptoteSlip = RearLeftCollider.sidewaysFriction.asymptoteSlip;
      _rLwheelFriction.asymptoteValue = RearLeftCollider.sidewaysFriction.asymptoteValue;
      _rLwheelFriction.stiffness = RearLeftCollider.sidewaysFriction.stiffness;
      _rRwheelFriction = new WheelFrictionCurve ();
      _rRwheelFriction.extremumSlip = RearRightCollider.sidewaysFriction.extremumSlip;
      _rrWextremumSlip = RearRightCollider.sidewaysFriction.extremumSlip;
      _rRwheelFriction.extremumValue = RearRightCollider.sidewaysFriction.extremumValue;
      _rRwheelFriction.asymptoteSlip = RearRightCollider.sidewaysFriction.asymptoteSlip;
      _rRwheelFriction.asymptoteValue = RearRightCollider.sidewaysFriction.asymptoteValue;
      _rRwheelFriction.stiffness = RearRightCollider.sidewaysFriction.stiffness;

      // We save the initial pitch of the car engine sound.
      if(CarEngineSound != null){
        _initialCarEngineSoundPitch = CarEngineSound.pitch;
      }

      // We invoke 2 methods inside this script. CarSpeedUI() changes the text of the UI object that stores
      // the speed of the car and CarSounds() controls the engine and drifting sounds. Both methods are invoked
      // in 0 seconds, and repeatedly called every 0.1 seconds.
      if(UseUI){
        InvokeRepeating("CarSpeedUI", 0f, 0.1f);
      }else if(!UseUI){
        if(CarSpeedText != null){
          CarSpeedText.text = "0";
        }
      }

      if(UseSounds){
        InvokeRepeating("CarSounds", 0f, 0.1f);
      }else if(!UseSounds){
        CarEngineSound?.Stop();
        if(TireScreechSound is not null){
          TireScreechSound.Stop();
        }
      }

      if(!UseEffects){
        RlwParticleSystem?.Stop();
        RrwParticleSystem?.Stop();
        if(RlwTireSkid is not null){
          RlwTireSkid.emitting = false;
        }
        if(RrwTireSkid is not null){
          RrwTireSkid.emitting = false;
        }
      }

      if(UseTouchControls){
        if(ThrottleButton != null && ReverseButton != null &&
           TurnRightButton != null && TurnLeftButton != null
           && HandbrakeButton != null){

          _throttlePti = ThrottleButton.GetComponent<PrometeoTouchInput>();
          _reversePti = ReverseButton.GetComponent<PrometeoTouchInput>();
          _turnLeftPti = TurnLeftButton.GetComponent<PrometeoTouchInput>();
          _turnRightPti = TurnRightButton.GetComponent<PrometeoTouchInput>();
          _handbrakePti = HandbrakeButton.GetComponent<PrometeoTouchInput>();
          _touchControlsSetup = true;

        }else{
          String ex = "Touch controls are not completely set up. You must drag and drop your scene buttons in the" +
                      " PrometeoCarController component.";
          Debug.LogWarning(ex);
        }
      }

    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.R))
      {
        carRespawnOnWaypoint.RespawnCar();
      }
      //CAR DATA

      // We determine the speed of the car.
      CarSpeed = (2 * Mathf.PI * FrontLeftCollider.radius * FrontLeftCollider.rpm * 60) / 1000;
      // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
      _localVelocityX = transform.InverseTransformDirection(_carRigidbody.linearVelocity).x;
      // Save the local velocity of the car in the z axis. Used to know if the car is going forward or backwards.
      _localVelocityZ = transform.InverseTransformDirection(_carRigidbody.linearVelocity).z;

      //CAR PHYSICS

      /*
      The next part is regarding to the car controller. First, it checks if the user wants to use touch controls (for
      mobile devices) or analog input controls (WASD + Space).

      The following methods are called whenever a certain key is pressed. For example, in the first 'if' we call the
      method GoForward() if the user has pressed W.

      In this part of the code we specify what the car needs to do if the user presses W (throttle), S (reverse),
      A (turn left), D (turn right) or Space bar (handbrake).
      */
      if (UseTouchControls && _touchControlsSetup){

        if(_throttlePti.ButtonPressed){
          CancelInvoke("DecelerateCar");
          _deceleratingCar = false;
          GoForward();
        }
        if(_reversePti.ButtonPressed){
          CancelInvoke("DecelerateCar");
          _deceleratingCar = false;
          GoReverse();
        }

        if(_turnLeftPti.ButtonPressed){
          TurnLeft();
        }
        if(_turnRightPti.ButtonPressed){
          TurnRight();
        }
        if(_handbrakePti.ButtonPressed){
          CancelInvoke("DecelerateCar");
          _deceleratingCar = false;
          Handbrake();
        }
        if(!_handbrakePti.ButtonPressed){
          RecoverTraction();
        }
        if((!_throttlePti.ButtonPressed && !_reversePti.ButtonPressed)){
          ThrottleOff();
        }
        if((!_reversePti.ButtonPressed && !_throttlePti.ButtonPressed) && !_handbrakePti.ButtonPressed && !_deceleratingCar){
          InvokeRepeating("DecelerateCar", 0f, 0.1f);
          _deceleratingCar = true;
        }
        if(!_turnLeftPti.ButtonPressed && !_turnRightPti.ButtonPressed && _steeringAxis != 0f){
          ResetSteeringAngle();
        }

      }else{

        if(Input.GetKey(KeyCode.W)){
          CancelInvoke("DecelerateCar");
          _deceleratingCar = false;
          GoForward();
        }
        if(Input.GetKey(KeyCode.S)){
          CancelInvoke("DecelerateCar");
          _deceleratingCar = false;
          GoReverse();
        }

        if(Input.GetKey(KeyCode.A)){
          TurnLeft();
        }
        if(Input.GetKey(KeyCode.D)){
          TurnRight();
        }
        if(Input.GetKey(KeyCode.Space)){
          CancelInvoke("DecelerateCar");
          _deceleratingCar = false;
          Handbrake();
        }
        if(Input.GetKeyUp(KeyCode.Space)){
          RecoverTraction();
        }
        if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))){
          ThrottleOff();
        }
        if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) && !_deceleratingCar){
          InvokeRepeating("DecelerateCar", 0f, 0.1f);
          _deceleratingCar = true;
        }
        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && _steeringAxis != 0f){
          ResetSteeringAngle();
        }

      }


      // We call the method AnimateWheelMeshes() in order to match the wheel collider movements with the 3D meshes of the wheels.
      AnimateWheelMeshes();

    }

    // This method converts the car speed data from float to string, and then set the text of the UI carSpeedText with this value.
    public void CarSpeedUI(){

      if(UseUI){
        try{
          float absoluteCarSpeed = Mathf.Abs(CarSpeed);
          CarSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }
      }

    }

    // This method controls the car sounds. For example, the car engine will sound slow when the car speed is low because the
    // pitch of the sound will be at its lowest point. On the other hand, it will sound fast when the car speed is high because
    // the pitch of the sound will be the sum of the initial pitch + the car speed divided by 100f.
    // Apart from that, the tireScreechSound will play whenever the car starts drifting or losing traction.
    public void CarSounds(){

      if(UseSounds){
        try{
          if(CarEngineSound != null){
            float engineSoundPitch = _initialCarEngineSoundPitch + (Mathf.Abs(_carRigidbody.linearVelocity.magnitude) / 25f);
            CarEngineSound.pitch = engineSoundPitch;
          }
          if((IsDrifting) || (IsTractionLocked && Mathf.Abs(CarSpeed) > 12f)){
            if(!TireScreechSound.isPlaying){
              TireScreechSound.Play();
            }
          }else if((!IsDrifting) && (!IsTractionLocked || Mathf.Abs(CarSpeed) < 12f)){
            TireScreechSound.Stop();
          }
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }
      }else if(!UseSounds){
        if(CarEngineSound != null && CarEngineSound.isPlaying){
          CarEngineSound.Stop();
        }
        if(TireScreechSound != null && TireScreechSound.isPlaying){
          TireScreechSound.Stop();
        }
      }

    }

    //
    //STEERING METHODS
    //

    //The following method turns the front car wheels to the left. The speed of this movement will depend on the steeringSpeed variable.
    public void TurnLeft(){
      _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * SteeringSpeed);
      if(_steeringAxis < -1f){
        _steeringAxis = -1f;
      }
      var steeringAngle = _steeringAxis * MaxSteeringAngle;
      FrontLeftCollider.steerAngle = Mathf.Lerp(FrontLeftCollider.steerAngle, steeringAngle, SteeringSpeed);
      FrontRightCollider.steerAngle = Mathf.Lerp(FrontRightCollider.steerAngle, steeringAngle, SteeringSpeed);
    }

    //The following method turns the front car wheels to the right. The speed of this movement will depend on the steeringSpeed variable.
    public void TurnRight(){
      _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * SteeringSpeed);
      if(_steeringAxis > 1f){
        _steeringAxis = 1f;
      }
      var steeringAngle = _steeringAxis * MaxSteeringAngle;
      FrontLeftCollider.steerAngle = Mathf.Lerp(FrontLeftCollider.steerAngle, steeringAngle, SteeringSpeed);
      FrontRightCollider.steerAngle = Mathf.Lerp(FrontRightCollider.steerAngle, steeringAngle, SteeringSpeed);
    }

    //The following method takes the front car wheels to their default position (rotation = 0). The speed of this movement will depend
    // on the steeringSpeed variable.
    public void ResetSteeringAngle(){
      if(_steeringAxis < 0f){
        _steeringAxis = _steeringAxis + (Time.deltaTime * 10f * SteeringSpeed);
      }else if(_steeringAxis > 0f){
        _steeringAxis = _steeringAxis - (Time.deltaTime * 10f * SteeringSpeed);
      }
      if(Mathf.Abs(FrontLeftCollider.steerAngle) < 1f){
        _steeringAxis = 0f;
      }
      var steeringAngle = _steeringAxis * MaxSteeringAngle;
      FrontLeftCollider.steerAngle = Mathf.Lerp(FrontLeftCollider.steerAngle, steeringAngle, SteeringSpeed);
      FrontRightCollider.steerAngle = Mathf.Lerp(FrontRightCollider.steerAngle, steeringAngle, SteeringSpeed);
    }

    // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
    void AnimateWheelMeshes(){
      try{
        Quaternion flwRotation;
        Vector3 flwPosition;
        FrontLeftCollider.GetWorldPose(out flwPosition, out flwRotation);
        FrontLeftMesh.transform.position = flwPosition;
        FrontLeftMesh.transform.rotation = flwRotation;

        Quaternion frwRotation;
        Vector3 frwPosition;
        FrontRightCollider.GetWorldPose(out frwPosition, out frwRotation);
        FrontRightMesh.transform.position = frwPosition;
        FrontRightMesh.transform.rotation = frwRotation;

        Quaternion rlwRotation;
        Vector3 rlwPosition;
        RearLeftCollider.GetWorldPose(out rlwPosition, out rlwRotation);
        RearLeftMesh.transform.position = rlwPosition;
        RearLeftMesh.transform.rotation = rlwRotation;

        Quaternion rrwRotation;
        Vector3 rrwPosition;
        RearRightCollider.GetWorldPose(out rrwPosition, out rrwRotation);
        RearRightMesh.transform.position = rrwPosition;
        RearRightMesh.transform.rotation = rrwRotation;
      }catch(Exception ex){
        Debug.LogWarning(ex);
      }
    }

    //
    //ENGINE AND BRAKING METHODS
    //

    // This method apply positive torque to the wheels in order to go forward.
    public void GoForward(){
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car is losing traction, then the car will start emitting particle systems.
      if(Mathf.Abs(_localVelocityX) > 2.5f){
        IsDrifting = true;
        DriftCarPS();
      }else{
        IsDrifting = false;
        DriftCarPS();
      }
      // The following part sets the throttle power to 1 smoothly.
      _throttleAxis = _throttleAxis + (Time.deltaTime * 3f);
      if(_throttleAxis > 1f){
        _throttleAxis = 1f;
      }
      //If the car is going backwards, then apply brakes in order to avoid strange
      //behaviours. If the local velocity in the 'z' axis is less than -1f, then it
      //is safe to apply positive torque to go forward.
      if(_localVelocityZ < -1f){
        Brakes();
      }else{
        if(Mathf.RoundToInt(CarSpeed) < MaxSpeed){
          //Apply positive torque in all wheels to go forward if maxSpeed has not been reached.
          FrontLeftCollider.brakeTorque = 0;
          FrontLeftCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          FrontRightCollider.brakeTorque = 0;
          FrontRightCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          RearLeftCollider.brakeTorque = 0;
          RearLeftCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          RearRightCollider.brakeTorque = 0;
          RearRightCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
        }else {
          // If the maxSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
          FrontLeftCollider.motorTorque = 0;
          FrontRightCollider.motorTorque = 0;
          RearLeftCollider.motorTorque = 0;
          RearRightCollider.motorTorque = 0;
        }
      }
    }

    // This method apply negative torque to the wheels in order to go backwards.
    public void GoReverse(){
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car is losing traction, then the car will start emitting particle systems.
      if(Mathf.Abs(_localVelocityX) > 2.5f){
        IsDrifting = true;
        DriftCarPS();
      }else{
        IsDrifting = false;
        DriftCarPS();
      }
      // The following part sets the throttle power to -1 smoothly.
      _throttleAxis = _throttleAxis - (Time.deltaTime * 3f);
      if(_throttleAxis < -1f){
        _throttleAxis = -1f;
      }
      //If the car is still going forward, then apply brakes in order to avoid strange
      //behaviours. If the local velocity in the 'z' axis is greater than 1f, then it
      //is safe to apply negative torque to go reverse.
      if(_localVelocityZ > 1f){
        Brakes();
      }else{
        if(Mathf.Abs(Mathf.RoundToInt(CarSpeed)) < MaxReverseSpeed){
          //Apply negative torque in all wheels to go in reverse if maxReverseSpeed has not been reached.
          FrontLeftCollider.brakeTorque = 0;
          FrontLeftCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          FrontRightCollider.brakeTorque = 0;
          FrontRightCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          RearLeftCollider.brakeTorque = 0;
          RearLeftCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
          RearRightCollider.brakeTorque = 0;
          RearRightCollider.motorTorque = (AccelerationMultiplier * 50f) * _throttleAxis;
        }else {
          //If the maxReverseSpeed has been reached, then stop applying torque to the wheels.
          // IMPORTANT: The maxReverseSpeed variable should be considered as an approximation; the speed of the car
          // could be a bit higher than expected.
          FrontLeftCollider.motorTorque = 0;
          FrontRightCollider.motorTorque = 0;
          RearLeftCollider.motorTorque = 0;
          RearRightCollider.motorTorque = 0;
        }
      }
    }

    //The following function set the motor torque to 0 (in case the user is not pressing either W or S).
    public void ThrottleOff(){
      FrontLeftCollider.motorTorque = 0;
      FrontRightCollider.motorTorque = 0;
      RearLeftCollider.motorTorque = 0;
      RearRightCollider.motorTorque = 0;
    }

    // The following method decelerates the speed of the car according to the decelerationMultiplier variable, where
    // 1 is the slowest and 10 is the fastest deceleration. This method is called by the function InvokeRepeating,
    // usually every 0.1f when the user is not pressing W (throttle), S (reverse) or Space bar (handbrake).
    public void DecelerateCar(){
      if(Mathf.Abs(_localVelocityX) > 2.5f){
        IsDrifting = true;
        DriftCarPS();
      }else{
        IsDrifting = false;
        DriftCarPS();
      }
      // The following part resets the throttle power to 0 smoothly.
      if(_throttleAxis != 0f){
        if(_throttleAxis > 0f){
          _throttleAxis = _throttleAxis - (Time.deltaTime * 10f);
        }else if(_throttleAxis < 0f){
          _throttleAxis = _throttleAxis + (Time.deltaTime * 10f);
        }
        if(Mathf.Abs(_throttleAxis) < 0.15f){
          _throttleAxis = 0f;
        }
      }
      _carRigidbody.linearVelocity = _carRigidbody.linearVelocity * (1f / (1f + (0.025f * DecelerationMultiplier)));
      // Since we want to decelerate the car, we are going to remove the torque from the wheels of the car.
      FrontLeftCollider.motorTorque = 0;
      FrontRightCollider.motorTorque = 0;
      RearLeftCollider.motorTorque = 0;
      RearRightCollider.motorTorque = 0;
      // If the magnitude of the car's velocity is less than 0.25f (very slow velocity), then stop the car completely and
      // also cancel the invoke of this method.
      if(_carRigidbody.linearVelocity.magnitude < 0.25f){
        _carRigidbody.linearVelocity = Vector3.zero;
        CancelInvoke("DecelerateCar");
      }
    }

    // This function applies brake torque to the wheels according to the brake force given by the user.
    public void Brakes(){
      FrontLeftCollider.brakeTorque = BrakeForce;
      FrontRightCollider.brakeTorque = BrakeForce;
      RearLeftCollider.brakeTorque = BrakeForce;
      RearRightCollider.brakeTorque = BrakeForce;
    }

    // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
    // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
    // it is high, then you could make the car to feel like going on ice.
    public void Handbrake(){
      CancelInvoke("RecoverTraction");
      // We are going to start losing traction smoothly, there is were our 'driftingAxis' variable takes
      // place. This variable will start from 0 and will reach a top value of 1, which means that the maximum
      // drifting value has been reached. It will increase smoothly by using the variable Time.deltaTime.
      _driftingAxis = _driftingAxis + (Time.deltaTime);
      float secureStartingPoint = _driftingAxis * _flWextremumSlip * HandbrakeDriftMultiplier;

      if(secureStartingPoint < _flWextremumSlip){
        _driftingAxis = _flWextremumSlip / (_flWextremumSlip * HandbrakeDriftMultiplier);
      }
      if(_driftingAxis > 1f){
        _driftingAxis = 1f;
      }
      //If the forces aplied to the rigidbody in the 'x' asis are greater than
      //3f, it means that the car lost its traction, then the car will start emitting particle systems.
      if(Mathf.Abs(_localVelocityX) > 2.5f){
        IsDrifting = true;
      }else{
        IsDrifting = false;
      }
      //If the 'driftingAxis' value is not 1f, it means that the wheels have not reach their maximum drifting
      //value, so, we are going to continue increasing the sideways friction of the wheels until driftingAxis
      // = 1f.
      if(_driftingAxis < 1f){
        _fLwheelFriction.extremumSlip = _flWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        FrontLeftCollider.sidewaysFriction = _fLwheelFriction;

        _fRwheelFriction.extremumSlip = _frWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        FrontRightCollider.sidewaysFriction = _fRwheelFriction;

        _rLwheelFriction.extremumSlip = _rlWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        RearLeftCollider.sidewaysFriction = _rLwheelFriction;

        _rRwheelFriction.extremumSlip = _rrWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        RearRightCollider.sidewaysFriction = _rRwheelFriction;
      }

      // Whenever the player uses the handbrake, it means that the wheels are locked, so we set 'isTractionLocked = true'
      // and, as a consequense, the car starts to emit trails to simulate the wheel skids.
      IsTractionLocked = true;
      DriftCarPS();

    }

    // This function is used to emit both the particle systems of the tires' smoke and the trail renderers of the tire skids
    // depending on the value of the bool variables 'isDrifting' and 'isTractionLocked'.
    public void DriftCarPS(){

      if(UseEffects){
        try{
          if(IsDrifting){
            RlwParticleSystem.Play();
            RrwParticleSystem.Play();
          }else if(!IsDrifting){
            RlwParticleSystem.Stop();
            RrwParticleSystem.Stop();
          }
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }

        try{
          if((IsTractionLocked || Mathf.Abs(_localVelocityX) > 5f) && Mathf.Abs(CarSpeed) > 12f){
            RlwTireSkid.emitting = true;
            RrwTireSkid.emitting = true;
          }else {
            RlwTireSkid.emitting = false;
            RrwTireSkid.emitting = false;
          }
        }catch(Exception ex){
          Debug.LogWarning(ex);
        }
      }else if(!UseEffects){
        RlwParticleSystem?.Stop();
        RrwParticleSystem?.Stop();
        if(RlwTireSkid is not null){
          RlwTireSkid.emitting = false;
        }
        if(RrwTireSkid is not null){
          RrwTireSkid.emitting = false;
        }
      }

    }

    // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
    public void RecoverTraction(){
      IsTractionLocked = false;
      _driftingAxis = _driftingAxis - (Time.deltaTime / 1.5f);
      if(_driftingAxis < 0f){
        _driftingAxis = 0f;
      }

      //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
      //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
      // car's grip.
      if(_fLwheelFriction.extremumSlip > _flWextremumSlip){
        _fLwheelFriction.extremumSlip = _flWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        FrontLeftCollider.sidewaysFriction = _fLwheelFriction;

        _fRwheelFriction.extremumSlip = _frWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        FrontRightCollider.sidewaysFriction = _fRwheelFriction;

        _rLwheelFriction.extremumSlip = _rlWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        RearLeftCollider.sidewaysFriction = _rLwheelFriction;

        _rRwheelFriction.extremumSlip = _rrWextremumSlip * HandbrakeDriftMultiplier * _driftingAxis;
        RearRightCollider.sidewaysFriction = _rRwheelFriction;

        Invoke("RecoverTraction", Time.deltaTime);

      }else if (_fLwheelFriction.extremumSlip < _flWextremumSlip){
        _fLwheelFriction.extremumSlip = _flWextremumSlip;
        FrontLeftCollider.sidewaysFriction = _fLwheelFriction;

        _fRwheelFriction.extremumSlip = _frWextremumSlip;
        FrontRightCollider.sidewaysFriction = _fRwheelFriction;

        _rLwheelFriction.extremumSlip = _rlWextremumSlip;
        RearLeftCollider.sidewaysFriction = _rLwheelFriction;

        _rRwheelFriction.extremumSlip = _rrWextremumSlip;
        RearRightCollider.sidewaysFriction = _rRwheelFriction;

        _driftingAxis = 0f;
      }
    }

  }
}
