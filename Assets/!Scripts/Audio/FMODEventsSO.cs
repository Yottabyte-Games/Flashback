using FMODUnity;
using UnityEngine;
namespace _Scripts.Audio
{
    [CreateAssetMenu(fileName = "FMODEventsSO", menuName = "Audio ScriptableObjects/FMODEventsSO", order = 1)]
    public class FMODEventsSO : ScriptableObject
    {
        [Header("Ambience")]
        public EventReference AmbienceHubworld;

        [Header("Player SFX")]
        public EventReference PlayerFootsteps;

        [Header("Car SFX")]
        public EventReference Checkpoint;
        public EventReference Finishline;

        [Header("Dialogue")]
        public EventReference Dialogue;
    }
}
