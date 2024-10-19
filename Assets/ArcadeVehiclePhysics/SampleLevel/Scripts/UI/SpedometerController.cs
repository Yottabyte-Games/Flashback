using ArcadeVehiclePhysics.Vehicle.Scripts.CA___CarAbilities;
using ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace ArcadeVehiclePhysics.SampleLevel.Scripts.UI
{
    public class SpedometerController : MonoBehaviour
    {
        public TextMeshProUGUI forwardSpeedText;
        public Image forwardSpeedBar;
        public TextMeshProUGUI sideSpeedText;
        public Image sideSpeedBar;
        public TextMeshProUGUI velMagText;
        public Image velMagBar;

        public CpMain cpMain;
        VehicleSpeed _speedData;

        CaBoost _caBoost;
        public Image boostBar;

        CaJump _caJump;
        public Image jumpCharge;

        // Start is called before the first frame update
        void Start()
        {
            if (cpMain == null)
            {
                cpMain = FindFirstObjectByType<CpMain>();
            }

            _caBoost = cpMain.GetComponent<CaBoost>();
            _caJump = cpMain.GetComponent<CaJump>();
        }

        // Update is called once per frame
        void Update()
        {
            _speedData = cpMain.speedData;

            float speedPercentage = _speedData.forwardSpeedPercent;
            UpdateSpeedBar(speedPercentage, forwardSpeedBar, forwardSpeedText, 900);

            float velMagPercentage = new Vector2(_speedData.forwardSpeed, _speedData.sideSpeed).magnitude / _speedData.topSpeed;
            UpdateSpeedBar(velMagPercentage, velMagBar, velMagText, 910);
            velMagText.rectTransform.localPosition += Vector3.up * 50;

            float sideSpeedPercentage = _speedData.sideSpeedPercent;
            UpdateSpeedBar(sideSpeedPercentage, sideSpeedBar, sideSpeedText, 900);


            if (_caBoost != null)
            {
                boostBar.rectTransform.sizeDelta = new Vector2(boostBar.rectTransform.sizeDelta.x, 900 * (_caBoost.currentBoostTimeLeft / _caBoost.boostTimeMax));
            }

            if (_caJump != null)
            {
                jumpCharge.rectTransform.sizeDelta = new Vector2(jumpCharge.rectTransform.sizeDelta.x, 900 * (_caJump.currentCharge / _caJump.forceMax));
            }
        }

        void UpdateSpeedBar(float valuePercentage, Image bar, TextMeshProUGUI text, int imageSize)
        {
            text.text = ((int)(valuePercentage * _speedData.topSpeed)).ToString();

            valuePercentage = Mathf.Clamp01(valuePercentage);
            text.rectTransform.localPosition = new Vector3(text.rectTransform.localPosition.x, -imageSize / 2 + valuePercentage * imageSize, text.rectTransform.localPosition.z);
            bar.rectTransform.sizeDelta = new Vector2(bar.rectTransform.sizeDelta.x, imageSize * valuePercentage);
        }

    }
}
