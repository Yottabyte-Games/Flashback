using System;
using GinjaGaming.FinalCharacterController;
using Imp_Assets.GinjaGaming.FinalCharacterController.Scripts;
using NaughtyAttributes;
using UnityEngine;

namespace _Scripts.Fishing
{
    public class Reel : MonoBehaviour
    {
        FishingRodInput input;
        FishingRod rod;
        PlayerController player;
        Bucket bucket;

        [ReadOnly, SerializeField] bool canReel;
        Fish toReel;

        public event Action FinishReel;
        [SerializeField] GameObject reelUI;

        Vector2 currentMousePos, lastMousePos, mouseMoved;

        private void Start()
        {
            player = FindFirstObjectByType<PlayerController>();
            bucket = FindFirstObjectByType<Bucket>();
            input = GetComponent<FishingRodInput>();
            rod = GetComponent<FishingRod>();
            input.Reel += ReelingValue;
            FinishReel += FinishReeling;
        }

        private void Update()
        {
            if (!canReel) return;

            rod.AddPullStrength(mouseMoved.magnitude * Time.deltaTime / 7);

            if (MathF.Abs(Vector3.Distance(rod.hook.transform.position, rod.hookPoint.transform.position)) <= 1)
            {
                bucket.AddFish(toReel);
                FinishReel.Invoke();
            }
        }
        void ReelingValue(Vector2 pos)
        {
            if (!canReel) return;

            if(currentMousePos != null)
            {
                lastMousePos = currentMousePos;
            }

            currentMousePos = pos;

            mouseMoved = currentMousePos - lastMousePos;
        }
        public void StartReeling(Fish fishToReel)
        {
            player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Confined;

            toReel = fishToReel;
            canReel = true;
            reelUI.SetActive(true);
        }
        void FinishReeling()
        {
            player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Locked;

            canReel = false;
            //toReel = new Fish();
            reelUI.SetActive(false);
        }
    }
}
