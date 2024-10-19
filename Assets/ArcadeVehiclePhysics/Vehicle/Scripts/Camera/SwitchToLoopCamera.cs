using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
namespace ArcadeVehiclePhysics.Vehicle.Scripts.Camera
{
    public class SwitchToLoopCamera : MonoBehaviour
    {
        //SwitchToLoopCamera.cs
        public CinemachineCamera loopCamera;

        [FormerlySerializedAs("PlayerOnLoop")]
        public bool playerOnLoop =false;
        float _offLoopTimerAmount = 0.2f;
        float _currentTimer = 0;

        // Update is called once per frame
        void Update()
        {
            if (!playerOnLoop && _currentTimer<=0)
            {
                loopCamera.Priority = 0;
            }
            else if (!playerOnLoop && _currentTimer>0)
            {
                _currentTimer -= Time.deltaTime;
            }
            else
            {
                loopCamera.Priority = 20;
            }
        }

        void OnCollisionEnter(Collision other)
        {
            playerOnLoop = true;
        }

        void OnCollisionExit(Collision other)
        {
            _currentTimer = _offLoopTimerAmount;
            playerOnLoop = false;
        }
    }
}
