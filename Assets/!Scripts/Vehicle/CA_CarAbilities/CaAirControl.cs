using System;
using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CA_CarAbilities
{
    public class CaAirControl : CaAbility
    {
    
        //Tweakable Variables
        public float airControlFactor;
        public Vector3 turnAxis;
        public float torqueAmount;
    
        //Internal Variables
        public float currentInput;

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
            currentInput = Input.GetAxis(axisKey);
        }

        public override void DoAbility()
        {
            if (Math.Abs(currentInput) < 0.01f)
                return;

            if (_cpMain.wheelData.grounded || _cpMain.averageColliderSurfaceNormal!=Vector3.zero)
                return;

            float rotationTorque = currentInput * torqueAmount * Time.fixedDeltaTime * airControlFactor;
            _cpMain.rb.AddRelativeTorque(turnAxis * rotationTorque, ForceMode.VelocityChange);
        }
    }
} 