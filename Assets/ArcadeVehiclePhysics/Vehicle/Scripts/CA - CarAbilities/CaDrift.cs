using ArcadeVehiclePhysics.Vehicle.Scripts.CP___CarPhysics;
using UnityEngine;
using UnityEngine.Serialization;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.CA___CarAbilities
{
    public class CaDrift : CaAbility
    {
        //Tweakable values
        public AnimationCurve driftCurve;
        float _driftInDuration = 0.5f;
        float _driftOutDuration = 1f;

        //Internal variables
        float _currentDriftTime;
        [FormerlySerializedAs("_currentDriftFactor")]
        public float currentDriftFactor;
        public bool isDrifting;


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

        void FixedUpdate()
        {
            //DoAbility();
        }

        public override void CheckInput()
        {
            if (_cpMain.wheelData.grounded)
            {
                isDrifting = Input.GetKey(abilityButton);
            }
        }

        void UpdateAbility()
        {
            if (isDrifting)
            {
                _currentDriftTime += Time.deltaTime * 1 / _driftInDuration;
            }
            else if (_currentDriftTime > 0)
            {
                _currentDriftTime -= Time.deltaTime * 1 / _driftOutDuration;
            }

            _currentDriftTime = Mathf.Clamp01(_currentDriftTime);
            currentDriftFactor = driftCurve.Evaluate(_currentDriftTime);
        
            _cpLateralFriction.currentTireStickiness =_cpLateralFriction.baseTireStickiness * currentDriftFactor;
        }

        public override void DoAbility()
        {
            bool belowBaseTireStickiness =  _cpLateralFriction.currentTireStickiness < _cpLateralFriction.baseTireStickiness;
    
            if (belowBaseTireStickiness && 
                !isDrifting && 
                _cpMain.wheelData.grounded
            )
            {
                //This is to try recover some lost speed while drifting
                _cpMain.rb.AddForce(Mathf.Abs(_cpLateralFriction.slidingFrictionForceAmount) * _cpMain.rb.transform.forward, ForceMode.Acceleration);
            }
        }
    }
}
