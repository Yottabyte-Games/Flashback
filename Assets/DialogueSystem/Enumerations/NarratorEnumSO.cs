using UnityEngine;
namespace DialogueSystem.Enumerations.StoryEnum
{
    //unity e-book: create-modular-game-architecture-in-unity-with-scriptableobjects
    [CreateAssetMenu(fileName = "NarratorEnumSO", menuName = "ScriptableObjects/NarratorEnumSO", order = 1)]
    public class NarratorEnumSO : ScriptableObject
    {
        public enum NarratorType
        {
            Player,
            Psychologist
        }

        public NarratorType narratorType;    
    }
}
