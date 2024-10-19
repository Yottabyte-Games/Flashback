using ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics;
using UnityEngine;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CA___CarAbilities
{
    public class CaBoost : CaAbility
    {
        //Tweakable Values
        public float boostForce = 20;
//    public AnimationCurve BoostCurve;
        public float boostTimeMax = 3;
        public float boostRechargeRate = 0.5f;

        //Internal Variables
        public float currentBoost = 0;
        public float currentBoostTimeLeft = 0;
        public bool isBoosting;

        CpMain _cpMain;
        CpAcceleration _cpAcceleration;

        void Awake()
        {
            _cpMain = GetComponent<CpMain>();
            _cpAcceleration = GetComponentInChildren<CpAcceleration>();
            currentBoostTimeLeft = boostTimeMax;
        }

        void Update()
        {
            CheckInput();
            UpdateAbility();
        }

        void FixedUpdate()
        {
            DoAbility();
        }

        public override void CheckInput()
        {
            isBoosting = Input.GetKey(abilityButton);
        }

        void UpdateAbility()
        {
            if (isBoosting && currentBoostTimeLeft > 0)
            {
                currentBoostTimeLeft -= Time.deltaTime;
                currentBoost = boostForce;
            }
            else if (currentBoostTimeLeft < boostTimeMax && !isBoosting)
            {
                currentBoostTimeLeft += boostRechargeRate * Time.deltaTime;
                currentBoostTimeLeft = Mathf.Clamp(currentBoostTimeLeft, 0, boostTimeMax);
            }
            else
            {
                currentBoost = 0;
            }
        }

        public override void DoAbility()
        {
            if (!_cpMain.wheelData.grounded)
                return;

            //Note sign has been accounted for when calculating acceleration
            Vector3 force = Time.fixedDeltaTime * currentBoost * _cpMain.rb.transform.forward; 
            _cpMain.rb.AddForce(force, ForceMode.Impulse);
        }
    }
}