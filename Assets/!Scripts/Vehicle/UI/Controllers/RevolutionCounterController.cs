using _Scripts.Vehicle.CP_CarPhysics;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Vehicle.UI.Controllers
{
    public class RevolutionCounterController : MonoBehaviour
    {
        [FormerlySerializedAs("RevText")]
        public TextMeshProUGUI revText;

        public float amountOfRotation;
        public float rotationInDegrees;
        [FormerlySerializedAs("NumberOfRevolutions")]
        public int numberOfRevolutions;

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = FindFirstObjectByType<CpMain>();
        }

        // Start is called before the first frame update
        void Start()
        {
            CpMain.OnLeavingGround += StartCountingRevs;
            CpMain.OnLanding += StopCountingRevolutions;
        }

        // Update is called once per frame
        void Update()
        {
            var isCountingRevolutions = _cpMain is not null;
            revText.gameObject.SetActive(isCountingRevolutions);

            if (!isCountingRevolutions || _cpMain.wheelData.grounded)
                return;
            amountOfRotation += _cpMain.rb.angularVelocity.y * Time.deltaTime;
            rotationInDegrees = amountOfRotation * 180 / Mathf.PI;
            numberOfRevolutions = (int)(rotationInDegrees / 180) * 180;
            revText.text = numberOfRevolutions.ToString();
        }

        public void StartCountingRevs(CpMain cpMain)
        {
            _cpMain = cpMain;
            amountOfRotation = 0;
        }

        public static void StopCountingRevolutions(CpMain cpMain)
        {
            Debug.Log("Stopping counting Revolutions");
        }
    }
}
