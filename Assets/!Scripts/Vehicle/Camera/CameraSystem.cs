using _Scripts.Vehicle.CP_CarPhysics;
using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Vehicle.Camera
{
    public class CameraSystem : MonoBehaviour
    {
        [Header("Camera")]
        public CinemachineCamera carCamGround;
        CinemachineFollow _carCamFollowGround;
        CinemachineBasicMultiChannelPerlin _camNoiseGround;

        public CinemachineCamera carCamAir;
        CinemachineFollow _carCamFollowAir;
        CinemachineBasicMultiChannelPerlin _camNoiseAir;

        public AnimationCurve carCamSpeedCurve;

        public CinemachineCamera currentVCam;
        public CinemachineFollow currentFollow;
        public CinemachineBasicMultiChannelPerlin currentNoise;

        public FloatRange fov = new FloatRange(30, 60);
        public FloatRange zPos = new FloatRange(-22, -16f);
        public FloatRange zDamp = new FloatRange(0.6f, 0.1f);
        public FloatRange yDamp = new FloatRange(0.8f, 0.4f);

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = GetComponent<CpMain>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _carCamFollowGround = carCamGround.GetComponent<CinemachineFollow>();
            _carCamFollowAir = carCamAir.GetComponent<CinemachineFollow>();
            _camNoiseGround = carCamGround.GetComponent<CinemachineBasicMultiChannelPerlin>();
            _camNoiseAir = carCamAir.GetComponent<CinemachineBasicMultiChannelPerlin>();

            CpMain.OnLanding += SwitchToGroundCamera;
            CpMain.OnLeavingGround += SwitchToAirCamera;

            SwitchToGroundCamera(_cpMain);
        }

        // Update is called once per frame
        void Update()
        {
            SpeedAdjustment();
        }

        public void SwitchToGroundCamera(CpMain cpMain)
        {
            if (cpMain.rb == _cpMain.rb)
            {
                carCamGround.Priority = 11;
                currentFollow = _carCamFollowGround;
                currentVCam = carCamGround;
                currentNoise = _camNoiseGround;
            }
        }

        public void SwitchToAirCamera(CpMain cpMain)
        {
            if (cpMain.rb == _cpMain.rb)
            {
                carCamGround.Priority = 9;
                currentFollow = _carCamFollowAir;
                currentVCam = carCamAir;
                currentNoise = _camNoiseAir;
            }
        }

        void SpeedAdjustment()
        {
            //Car cam speed settings
            var currentSpeedPercentage = carCamSpeedCurve.Evaluate(_cpMain.speedData.forwardSpeedPercent);

            currentVCam.Lens.FieldOfView = fov.GetCurrentWithPercent(currentSpeedPercentage);

            zPos.GetCurrentWithPercent(currentSpeedPercentage);
            currentFollow.FollowOffset = new Vector3(0, 5, zPos.current);
            currentFollow.TrackerSettings.PositionDamping = new Vector3(
                currentFollow.TrackerSettings.PositionDamping.x,
                yDamp.GetCurrentWithPercent(currentSpeedPercentage),
                zDamp.GetCurrentWithPercent(currentSpeedPercentage)
            );
        }
    }

    [System.Serializable]
    public class FloatRange
    {
        public float min;
        public float max;
        public float current;

        public float range => (max - min);

        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetCurrentWithPercent(float percentage)
        {
            current = min + percentage * range;
            return current;
        }
    }
}