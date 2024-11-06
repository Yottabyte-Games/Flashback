using System;
using System.Threading.Tasks;
using GinjaGaming.FinalCharacterController;
using Minigame.Fishing;
using NaughtyAttributes;
using UnityEngine;
using Utility.Math;
using Utility.Physics;

namespace Minigame.Fishing
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
        float reelValue;

        private void Start()
        {
            player = FindFirstObjectByType<PlayerController>();
            bucket = FindFirstObjectByType<Bucket>();
            input = GetComponent<FishingRodInput>();
            rod = GetComponent<FishingRod>();
            input.Reel += ReelingValue;
            FinishReel += FinishReeling;
        }

        async void ReelingValue(Vector2 pos)
        {
            if (!canReel) return;

            if(currentMousePos != null)
            {
                lastMousePos = currentMousePos;
            }

            currentMousePos = pos;

            mouseMoved = currentMousePos - lastMousePos;

            reelValue += mouseMoved.magnitude / 2000;
            print(reelValue + " " + toReel.Difficulty);

            if (reelValue >= toReel.Difficulty)
            {
                await UPhysics.ThrowToAsync(rod.hook.rb, rod.hookPoint.position);

                bucket.AddFish(toReel);
                FinishReel.Invoke();
                rod.hook.fish = null;
            }
        }
        public void StartReeling(Fish fishToReel)
        {
            player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Confined;

            toReel = fishToReel;
            canReel = true;
            reelUI.SetActive(true);
            reelValue = 0;
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
