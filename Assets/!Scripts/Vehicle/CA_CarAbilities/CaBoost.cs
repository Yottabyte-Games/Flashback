using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CA_CarAbilities
{
    public class CaBoost : CaAbility
    {
        //Tweak-able Values
        public float boostForce = 20; 
        public float boostTimeMax = 3;
        public float boostRechargeRate = 0.5f;

        //Internal Variables
        [HideInInspector]
        public float currentBoost;
        [HideInInspector] 
        public float currentBoostTimeLeft;
        [HideInInspector]
        public bool isBoosting;

        CpMain _cpMain;

        void Awake()
        {
            _cpMain = GetComponent<CpMain>();
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
            else if (currentBoostTimeLeft < boostTimeMax)
            {
                switch (isBoosting)
                {
                    case false:
                        currentBoostTimeLeft += boostRechargeRate * Time.deltaTime;
                        currentBoostTimeLeft = Mathf.Clamp(currentBoostTimeLeft, 0, boostTimeMax);
                        break;
                    default:
                        currentBoost = 0;
                        break;
                }
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
            var force = Time.fixedDeltaTime * currentBoost * _cpMain.rb.transform.forward; 
            _cpMain.rb.AddForce(force, ForceMode.Impulse);
        }
    }
}