using UnityEngine;

namespace YottabyteGames.Scripts.Scriptable_Objects
{    
    [CreateAssetMenu(fileName = "InteractionInputData", menuName = "FirstPersonController/InteractionSystem/InputData")]
    public class InteractionInputData : ScriptableObject
    {
        private bool m_interactedClicked;
        private bool m_interactedRelease;

        public bool InteractedClicked
        {
            get => m_interactedClicked;
            set => m_interactedClicked = value;
        }

        public bool InteractedReleased
        {
            get => m_interactedRelease;
            set => m_interactedRelease = value;
        }

        public void ResetInput()
        {
            m_interactedClicked = false;
            m_interactedRelease = false;
        }
    }
}
