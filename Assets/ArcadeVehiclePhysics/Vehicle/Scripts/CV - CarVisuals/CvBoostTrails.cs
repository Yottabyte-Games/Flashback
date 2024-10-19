using ArcadeVehiclePhysics.Vehicle.Scripts.CA___CarAbilities;
using ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics;
using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CV___CarVisuals
{
    public class CvBoostTrails : MonoBehaviour
    {
        public TrailRenderer leftLightTrail;
        public TrailRenderer rightLightTrail;


        CaBoost _caBoost;
        CpMain _cpMain;
        // Start is called before the first frame update

        void Awake()
        {
            _caBoost = transform.parent.GetComponent<CaBoost>();
            _cpMain = transform.parent.GetComponent<CpMain>();
        }

        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_caBoost!=null)
            {
                leftLightTrail.emitting = _caBoost.isBoosting || _cpMain.speedData.forwardSpeedPercent>1;
                rightLightTrail.emitting = _caBoost.isBoosting|| _cpMain.speedData.forwardSpeedPercent>1;
            }
        }
    }
}
