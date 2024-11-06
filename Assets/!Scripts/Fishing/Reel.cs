using GinjaGaming.FinalCharacterController;
using NaughtyAttributes;
using System;
using UnityEngine;

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
        }

        void ReelingValue(Vector2 pos)
        {
            if (!canReel) return;

            if (currentMousePos != null)
            {
                lastMousePos = currentMousePos;
            }

            currentMousePos = pos;

            mouseMoved = currentMousePos - lastMousePos;

            reelValue += mouseMoved.magnitude / 2000;

            if (reelValue >= toReel.Difficulty)
            {
                FinishReeling();
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
        async void FinishReeling()
        {
            player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Locked;

            canReel = false;
            bucket.AddFish(toReel);
            rod.hook.fish = null;
            reelUI.SetActive(false);

            await rod.ReelHook();

            FinishReel.Invoke();
        }
    }
}
