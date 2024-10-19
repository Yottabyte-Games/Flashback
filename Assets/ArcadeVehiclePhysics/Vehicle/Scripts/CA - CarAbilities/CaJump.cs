using ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics;
using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CA___CarAbilities
{
    public class CaJump : CaAbility
    {
        //Tweakable Variables
        public float forceMin = 20;
        public float forceMax = 50;
        public float forceChargeRate = 15;

        [Space]
        //Internal Variables
        public float currentCharge = 0;
        public bool jumpPressed;

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = GetComponent<CpMain>();
        }

        void Update()
        {
            CheckInput();

        }

        void FixedUpdate()
        {
            DoAbility();
        }

        public override void CheckInput()
        {
            if (Input.GetKey(abilityButton))
            {
                currentCharge += forceChargeRate * Time.deltaTime;
                currentCharge = Mathf.Clamp(currentCharge, forceMin, forceMax);
            }
            if (Input.GetKeyUp(abilityButton))
            {
                jumpPressed = true;
            }
        }

        public override void DoAbility()
        {
            if (!jumpPressed) return;

            jumpPressed = false;

            //Remove if you want air jumps
            if (!_cpMain.wheelData.grounded) return;

            Rigidbody rb = _cpMain.rb;
            rb.AddForceAtPosition(currentCharge * rb.transform.up, rb.position, ForceMode.Impulse);
            currentCharge = 0;
        
        }

    }
}
