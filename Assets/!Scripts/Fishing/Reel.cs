using System;
using GinjaGaming.FinalCharacterController;
using Minigame.Fishing;
using NaughtyAttributes;
using UnityEngine;
using Utility.Math;

namespace Minigame.Fishing
{
    public class Reel : MonoBehaviour
    {
        FishingRodInput input;
        Casting cast;
        PlayerController player;
        Bucket bucket;

        [ReadOnly, SerializeField] bool canReel;
        Fish toReel;

        Vector2 lastReelPos, currentReelPos, difference;
        float travelDistance;
        Vector3 hookStartPos, hookEndPos;

        public event Action FinishReel;

        private void Start()
        {
            player = FindFirstObjectByType<PlayerController>();
            bucket = FindFirstObjectByType<Bucket>();
            input = GetComponent<FishingRodInput>();
            cast = GetComponent<Casting>();
            input.reel += ReelingValue;
            FinishReel += FinishReeling;
        }

        private void Update()
        {
            if (!canReel) return;

            Vector3 lerpValue = UMath.LerpVector3(hookStartPos, hookEndPos, travelDistance / toReel.difficulty);
            cast.currentHook.transform.position = new Vector3(lerpValue.x, cast.currentHook.transform.position.y, lerpValue.z);

            if(travelDistance / toReel.difficulty >= .65f)
            {
                bucket.AddFish(toReel);
                FinishReel.Invoke();
            }
        }
        void ReelingValue(Vector2 pos)
        {
            if (!canReel) return;

            if (currentReelPos != null)
                lastReelPos = currentReelPos;

            currentReelPos = pos;

            if(lastReelPos != null && currentReelPos != null)
            {
                difference = currentReelPos - lastReelPos;
            }


            travelDistance += difference.magnitude / 37.7f / 100;
        }
        public void StartReeling(Fish fishToReel)
        {
            player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Confined;

            hookStartPos = cast.currentHook.transform.position;
            hookEndPos = transform.position;
            toReel = fishToReel;
            canReel = true;
        }
        void FinishReeling()
        {
            player.ToggleCameraMovement();
            Cursor.lockState = CursorLockMode.Locked;

            canReel = false;
            toReel = new Fish();
            travelDistance = 0;
            lastReelPos = Vector2.zero;
            currentReelPos = Vector2.zero;
            difference = Vector2.zero;
        }
    }
}
