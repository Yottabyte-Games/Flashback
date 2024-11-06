using System;
using GinjaGaming.FinalCharacterController;
using Imp_Assets.GinjaGaming.FinalCharacterController.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Fishing
{
    public class Reel : MonoBehaviour
    {
        FishingRodInput _input;
        FishingRod _rod;
        PlayerController _player;
        Bucket _bucket;

        [ReadOnly, SerializeField] bool canReel;
        Fish _toReel;

        public event Action FinishReel;
        [SerializeField] GameObject reelUI;

        Vector2 _currentMousePos, _lastMousePos, _mouseMoved;

        private void Start()
        {
            _player = FindFirstObjectByType<PlayerController>();
            _bucket = FindFirstObjectByType<Bucket>();
            _input = GetComponent<FishingRodInput>();
            _rod = GetComponent<FishingRod>();
            _input.Reel += ReelingValue;
            FinishReel += FinishReeling;
        }

        private void Update()
        {
            if (!canReel) return;

            _rod.AddPullStrength(_mouseMoved.magnitude * Time.deltaTime / 7);

            if (MathF.Abs(Vector3.Distance(_rod.hook.transform.position, _rod.hookPoint.transform.position)) <= 1)
            {
                _bucket.AddFish(_toReel);
                FinishReel.Invoke();
            }
        }
        void ReelingValue(Vector2 pos)
        {
            if (!canReel) return;

            if(_currentMousePos != null)
            {
                _lastMousePos = _currentMousePos;
            }

            _currentMousePos = pos;

            _mouseMoved = _currentMousePos - _lastMousePos;
        }
        public void StartReeling(Fish fishToReel)
        {
            _player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Confined;

            _toReel = fishToReel;
            canReel = true;
            reelUI.SetActive(true);
        }
        void FinishReeling()
        {
            _player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Locked;

            canReel = false;
            //toReel = new Fish();
            reelUI.SetActive(false);
        }
    }
}
