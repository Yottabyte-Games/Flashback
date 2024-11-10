using TMPro;
using UnityEngine;
using Utility.Methods;

namespace _Scripts.Fishing
{

    public class FishDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text fishInfo;
        Fish fishToDisplay;
        [SerializeField] string additionalInfo;

        private void Start()
        {
            fishInfo.text = null;
        }

        public void DisplayFish(Fish fish)
        {
            fishToDisplay = fish;

            fish.transform.parent = transform;

            UMethods.ResetTransform(fish.transform, true);
            fish.gameObject.SetActive(true);
            
            fishInfo.text = additionalInfo + 
                "Fish: " + fish.type + 
                "\nWeight: " + fish.Weight.ToString("0.00") + 
                "\nLength: " + fish.Length.ToString("0.00");
        }
        public void RemoveFish()
        {
            if(fishToDisplay == null) return;

            fishToDisplay.gameObject.SetActive(false);
            fishInfo.text = null;
        }
    }
}
