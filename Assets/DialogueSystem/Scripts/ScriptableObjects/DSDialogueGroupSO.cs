using UnityEngine;

namespace DialogueSystem.Scripts.ScriptableObjects
{
    public class DSDialogueGroupSo : ScriptableObject
    {
        [field: SerializeField] public string groupName { get; set; }

        public void Initialize(string groupName)
        {
            this.groupName = groupName;
        }
    }
}