using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.FPS.Interaction_System
{        
    public class InteractionUIPanel : MonoBehaviour
    {
        [SerializeField] Image progressBar;
        [SerializeField] TextMeshProUGUI tooltipText;

        public void SetTooltip(string tooltip)
        {
            tooltipText.SetText(tooltip);
        }

        public void UpdateProgressBar(float fillAmount)
        {
            progressBar.fillAmount = fillAmount;
        }

        public void ResetUI()
        {
            progressBar.fillAmount = 0f;
            tooltipText.SetText("");
        }
    }
}
