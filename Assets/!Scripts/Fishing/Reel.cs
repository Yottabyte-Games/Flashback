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
        
        [SerializeField] DialogueStarter[] ds;
        [SerializeField] GameObject reelUI;
        [SerializeField] Slider indicator;

        Vector2 currentMousePos, lastMousePos, mouseMoved;
        float reelValue;
        int reelCounter;

        private void Awake()
        {
            input = GetComponent<FishingRodInput>();
            rod = GetComponent<FishingRod>();
        }
        void OnEnable()
        {
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
        }
        async void FinishReeling()
        {
            Cursor.lockState = CursorLockMode.Locked;
            reelUI.SetActive(false);

            await rod.ReelHook();

            PlayDialogue();
            
            rod.ToggleReeling(false);
        }

        //Sorry Torje
        private void PlayDialogue()
        {
            ds[reelCounter].StartDialogue();
            reelCounter++;
        }
    }
}
