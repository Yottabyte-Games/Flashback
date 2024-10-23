using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CV_CarVisuals
{
    public class CvBodyMovement : MonoBehaviour
    {
        public Transform modelBase;
        public Vector3 modelBaseOffset;

        public BodyAxisMovement roll;
        public BodyAxisMovement pitch;

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = transform.parent.GetComponentInChildren<CpMain>();
        }
        
        // Update is called once per frame
        void Update()
        {
            UpdateCarBodyTransform();
        }

        void UpdateCarBodyTransform()
        {
            transform.position = _cpMain.rb.position;
            transform.rotation = _cpMain.rb.rotation;

            //CarBody Roll - Steering and Side Speed
            roll.currentAngle = Mathf.Lerp(roll.currentAngle, roll.inputMaxAngle * _cpMain.input.steeringInput, Time.deltaTime*10);
            var currentBodyRoll = roll.currentAngle - _cpMain.speedData.sideSpeedPercent * roll.speedMaxAngle;
        
            //CarBody Pitch - Accel and Forward Speed
            pitch.currentAngle = Mathf.Lerp(pitch.currentAngle, _cpMain.input.accelInput * pitch.inputMaxAngle, Time.deltaTime*10);
            var currentBodyPitch = pitch.currentAngle + Mathf.Clamp01(_cpMain.speedData.forwardSpeedPercent) * pitch.speedMaxAngle;

            modelBase.rotation = _cpMain.rb.rotation * Quaternion.Euler(currentBodyPitch, 0, currentBodyRoll);
        }
    }

    [System.Serializable]
    public class BodyAxisMovement
    {
        public float currentAngle;
        public float inputMaxAngle;
        public float speedMaxAngle;
    }
}