using _Scripts.Rive;
using GinjaGaming.FinalCharacterController;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Fishing
{
    public class Reel : MonoBehaviour
    {
        FishingRodInput input;
        FishingRod rod;
        Fish toReel;
        
        GameHudController gameHud;
       
        [SerializeField] GameObject reelUI;
        [SerializeField] Slider indicator;

        Vector2 currentMousePos, lastMousePos, mouseMoved;
        float reelValue;

        bool finishReel;

        void Awake()
        {
            input = GetComponent<FishingRodInput>();
            rod = GetComponent<FishingRod>();
            gameHud = GameObject.FindWithTag("MainCamera").GetComponent<GameHudController>();
        }
        void OnEnable()
        {
            finishReel = false;
            input.Reel += ReelingValue;

            if (rod.hook.fish)
            StartReeling(rod.hook.fish);
        }

        void OnDisable()
        {
            input.Reel -= ReelingValue;
            FinishReeling();
        }

        void ReelingValue(Vector2 pos)
        {
            if (finishReel) return;

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

            indicator.value = reelValue;
        }
        public void StartReeling(Fish fishToReel)
        {
            Cursor.lockState = CursorLockMode.Confined;

            toReel = fishToReel;
            reelUI.SetActive(true);
            reelValue = 0;

            indicator.value = 0;
            indicator.maxValue = toReel.Difficulty;
            
            gameHud.SetCursorHidden(true);
        }
        async void FinishReeling()
        {
            finishReel = true;
            Cursor.lockState = CursorLockMode.Locked;
            reelUI.SetActive(false);

            await rod.ReelHook();
            rod.ToggleReeling(false);
            rod.FishCaught.Invoke();
            gameHud.SetCursorHidden(false);
        }
    }
}
