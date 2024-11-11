using GinjaGaming.FinalCharacterController;
using UnityEngine;

namespace _Scripts.Fishing
{
    public class Reel : MonoBehaviour
    {
        FishingRodInput input;
        FishingRod rod;
        Fish toReel;

        [SerializeField] GameObject reelUI;

        Vector2 currentMousePos, lastMousePos, mouseMoved;
        float reelValue;

        void OnEnable()
        {
            input = GetComponent<FishingRodInput>();
            rod = GetComponent<FishingRod>();

            input.Reel += ReelingValue;
            StartReeling(rod.hook.fish);
        }

        void OnDisable()
        {
            input.Reel -= ReelingValue;
            FinishReeling();
        }

        void ReelingValue(Vector2 pos)
        {
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
            Cursor.lockState = CursorLockMode.Confined;

            toReel = fishToReel;
            reelUI.SetActive(true);
            reelValue = 0;
        }
        async void FinishReeling()
        {
            Cursor.lockState = CursorLockMode.Locked;
            reelUI.SetActive(false);

            await rod.ReelHook();

            rod.ToggleReeling(false);
        }
    }
}
