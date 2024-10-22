using ArcadeVehiclePhysics.Vehicle.Scripts.Camera;
using ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics;
using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CV___CarVisuals
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