using _Scripts.Vehicle.CA_CarAbilities;
using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace _Scripts.Vehicle.CV_CarVisuals
{
    public class CvBoostPostProcessingEffect : MonoBehaviour
    {
        [FormerlySerializedAs("_profile")]
        public PostProcessProfile profile;
        ChromaticAberration _chromaticAberration;
        LensDistortion _lensDistortion;

        CpMain _cpMain;
        CaBoost _caBoost;

        void Awake()
        {
            _cpMain = transform.parent.GetComponent<CpMain>();
            _caBoost = transform.parent.GetComponent<CaBoost>();
        
            _chromaticAberration = profile.GetSetting<ChromaticAberration>();
            _lensDistortion = profile.GetSetting<LensDistortion>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_caBoost.isBoosting)
            {

                _chromaticAberration.intensity.value =
                    Mathf.Lerp(_chromaticAberration.intensity.value, 1, Time.deltaTime * 5);
                _lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, -20f, Time.deltaTime * 5);
            }
            else
            {
                _chromaticAberration.intensity.value =
                    Mathf.Lerp(_chromaticAberration.intensity.value, 0, Time.deltaTime *10);
                _lensDistortion.intensity.value = Mathf.Lerp(_lensDistortion.intensity.value, 0f, Time.deltaTime * 10);
            }
        }
    }
}
