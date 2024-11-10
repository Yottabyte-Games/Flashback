using System;
using UnityEngine;

namespace _Scripts.Vehicle.CP_CarPhysics
{
    public class CpAcceleration : MonoBehaviour
    {
        [Header("Acceleration")] 
        public AnimationCurve velocityTimeCurve;

        public float accelerationToApply;
        public float currentTimeValue;
        public float nextTimeValue;
        public float nextVelocityMagnitude;

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = transform.parent.GetComponent<CpMain>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _cpMain.speedData = new VehicleSpeed(velocityTimeCurve.keys[velocityTimeCurve.length - 1].value);
        }

        // Update is called once per frame
        void Update()
        {
            CalculateSpeedData(_cpMain.rb, _cpMain.speedData);
            accelerationToApply = GetAccelerationFromVelocityTimeCurve(velocityTimeCurve, _cpMain.input, _cpMain.speedData);
        }

        void FixedUpdate()
        {
            var inputScaledAccel = Mathf.Abs(_cpMain.input.accelInput) * accelerationToApply;
            ApplyAcceleration(inputScaledAccel, _cpMain.rb, _cpMain.wheelData.grounded);
        }

        static void CalculateSpeedData(Rigidbody rb, VehicleSpeed speedData)
        {
            speedData.sideSpeed = Vector3.Dot(rb.transform.right, rb.linearVelocity);
            speedData.forwardSpeed = Vector3.Dot(rb.transform.forward, rb.linearVelocity);
            speedData.speed = rb.linearVelocity.magnitude;
        }

        //An Alternative to the Vel-Time Curve approach
        //This works by adjusting the force applied according to how fast the car is moving
        //Top speed is defined on the curve by the value of the first key
       static float GetForceFromVelocityForceCurve(AnimationCurve velocityForceCurve, PlayerInputs input,
            VehicleSpeed speedData)
        {
            if (Math.Abs(input.accelInput) < 0.05f)
                return 0;

            var curveTopSpeed = velocityForceCurve.keys[0].value;
            var velocityForceCurveEvaluation = velocityForceCurve.Evaluate(speedData.forwardSpeed / curveTopSpeed);

            return velocityForceCurveEvaluation * curveTopSpeed;
        }

        //This process works using reverse evaluation of a Velocity-Time curve
        //Binary search using current forward speed to find the time value on the graph
        //Add one time step onto that time value and evaluate the graph to get the new velocity
        //Calculate a = (Vf - Vi)/deltaTime
        //Return the new force to apply (Must use ForceMode.Acceleration to ignore mass)
        //timeScaler is the scale of the time buckets used in the binary search as it uses Ints
        float GetAccelerationFromVelocityTimeCurve(AnimationCurve velocityTime, PlayerInputs input,
            VehicleSpeed speedData)
        {
            if (speedData.forwardSpeed > velocityTime.keys[velocityTime.length - 1].value)
                return 0;

            var speedClamped = Mathf.Clamp(
                speedData.forwardSpeed,
                velocityTime.keys[0].value,
                velocityTime.keys[velocityTime.length - 1].value);

            currentTimeValue = BinarySearchDisplay(velocityTime, speedClamped);

            switch (Mathf.Approximately(currentTimeValue, -1))
            {
                case false:
                {
                    float inputDir = input.accelInput > 0 ? 1 : -1;
                    nextTimeValue = currentTimeValue + inputDir * Time.fixedDeltaTime;
                    nextTimeValue = Mathf.Clamp(nextTimeValue, velocityTime.keys[0].time,
                        velocityTime.keys[velocityTime.length - 1].time);

                    nextVelocityMagnitude = velocityTime.Evaluate(nextTimeValue);
                    var accelMagnitude = (nextVelocityMagnitude - speedData.forwardSpeed) / (Time.fixedDeltaTime);

                    return accelMagnitude;
                }
                default:
                    return 0;
            }
        }

        static float BinarySearchDisplay(AnimationCurve velTimeCurve, float currentVel)
        {
            const int timeScale = 10000;
        
            var minTime = (int)(velTimeCurve.keys[0].time * timeScale);
            var maxTime = (int)(velTimeCurve.keys[velTimeCurve.length - 1].time * timeScale);
            var numSteps = 0;

            while (minTime <= maxTime)
            {
                var mid = (minTime + maxTime) / 2;

                var scaledMid = (float) mid / timeScale;
                if (Mathf.Abs(velTimeCurve.Evaluate(scaledMid) - currentVel) <= 0.01f)
                {
                    //Debug.Log(string.Format("Final mid: {0}", mid));
                    return (float)mid/timeScale;
                }
            
                if (currentVel < velTimeCurve.Evaluate(scaledMid))
                {
                    maxTime = mid - 1;
                }
                else
                {
                    minTime = mid + 1;
                }

                //Debug.Log(string.Format("minTime: {0}   maxTime:{1}   mid: {2}   numSteps: {3}", minTime, maxTime, mid, numSteps));
                numSteps += 1;
            }

            //Debug.Log("[BinarySearchDisplay] Something went wrong with the BinarySearch - Returning -1");
            return -1;
        }

        static void ApplyAcceleration(float accelToApply, Rigidbody rb, bool grounded)
        {
            if (!grounded)
                return;

            //Note sign has been accounted for when calculating acceleration
            var force = rb.transform.forward * accelToApply; 
            rb.AddForce(force, ForceMode.Acceleration);
        }
    }

    [Serializable]
    public class VehicleSpeed
    {
        public float speed;
        public float forwardSpeed;
        public float sideSpeed;
        public float topSpeed;

        //Percent of top speed
        public float forwardSpeedPercent => Mathf.Abs(forwardSpeed / topSpeed);
        public float sideSpeedPercent => Mathf.Abs(sideSpeed / topSpeed);
        public float speedPercent => Mathf.Abs(speed / topSpeed);

        public VehicleSpeed(float topSpeed)
        {
            this.topSpeed = topSpeed;
        }
    }
}