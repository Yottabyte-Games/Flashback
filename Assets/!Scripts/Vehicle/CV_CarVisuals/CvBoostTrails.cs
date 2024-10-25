using _Scripts.Vehicle.CA_CarAbilities;
using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CV_CarVisuals
{
    public class CvBoostTrails : MonoBehaviour
    {
        public TrailRenderer leftLightTrail;
        public TrailRenderer rightLightTrail;


        CaBoost _caBoost;
        CpMain _cpMain;
        
        void Awake()
        {
            _caBoost = transform.parent.GetComponent<CaBoost>();
            _cpMain = transform.parent.GetComponent<CpMain>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_caBoost is null) return;
            leftLightTrail.emitting = _caBoost.isBoosting || _cpMain.speedData.forwardSpeedPercent>1;
            rightLightTrail.emitting = _caBoost.isBoosting|| _cpMain.speedData.forwardSpeedPercent>1;
        }
    }
}
