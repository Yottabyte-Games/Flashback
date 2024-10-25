using _Scripts.Vehicle.Camera;
using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CV_CarVisuals
{
    public class CvSpeedCameraShake : MonoBehaviour
    {
        //CvSpeedCameraShake.cs
        CpMain _cpMain;
        CameraSystem _cameraSystem;


        void Awake()
        {
            _cpMain = transform.parent.GetComponent<CpMain>();
            _cameraSystem = transform.parent.GetComponent<CameraSystem>();
        }
        
        // Update is called once per frame
        void Update()
        {
            _cameraSystem.currentNoise.FrequencyGain = (_cpMain.speedData.speedPercent - 0.3f) * 15f;
            _cameraSystem.currentNoise.AmplitudeGain = Mathf.Clamp01(_cpMain.speedData.speedPercent - 0.98f);
        }
    }
}