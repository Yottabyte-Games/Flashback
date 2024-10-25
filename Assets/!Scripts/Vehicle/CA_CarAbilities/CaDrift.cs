using _Scripts.Vehicle.CP_CarPhysics;
using UnityEngine;

namespace _Scripts.Vehicle.CA_CarAbilities
{
    public class CaDrift : CaAbility
    {
        //Tweakable values
        public AnimationCurve driftCurve;
        [SerializeField] float driftInDuration = 0.5f;
        [SerializeField] float driftOutDuration = 1f;

        //Internal variables
        float _currentDriftTime;
        float _currentDriftFactor;
        bool _isDrifting;


        CpMain _cpMain;
        CpLateralFriction _cpLateralFriction;

        void Awake()
        {
            _cpMain = GetComponent<CpMain>();
            _cpLateralFriction = GetComponentInChildren<CpLateralFriction>();
        }

        void Update()
        {
            CheckInput();
            UpdateAbility();
        }
        
        public override void CheckInput()
        {
            if (_cpMain.wheelData.grounded)
            {
                _isDrifting = Input.GetKey(abilityButton);
            }
        }

        void UpdateAbility()
        {
            if (_isDrifting)
            {
                _currentDriftTime += Time.deltaTime * 1 / driftInDuration;
            }
            else if (_currentDriftTime > 0)
            {
                _currentDriftTime -= Time.deltaTime * 1 / driftOutDuration;
            }

            _currentDriftTime = Mathf.Clamp01(_currentDriftTime);
            _currentDriftFactor = driftCurve.Evaluate(_currentDriftTime);
        
            _cpLateralFriction.currentTireStickiness =_cpLateralFriction.baseTireStickiness * _currentDriftFactor;
        }

        public override void DoAbility()
        {
            var belowBaseTireStickiness =  _cpLateralFriction.currentTireStickiness < _cpLateralFriction.baseTireStickiness;

            switch (belowBaseTireStickiness)
            {
                case true when 
                    !_isDrifting && 
                    _cpMain.wheelData.grounded:
                    //This is to try recover some lost speed while drifting
                    _cpMain.rb.AddForce(Mathf.Abs(_cpLateralFriction.slidingFrictionForceAmount) * _cpMain.rb.transform.forward, ForceMode.Acceleration);
                    break;
            }
        }
    }
}
