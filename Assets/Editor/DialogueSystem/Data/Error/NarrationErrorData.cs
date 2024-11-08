using UnityEngine;

namespace Editor.DialogueSystem.Data.Error
{
    public class NarrationErrorData
    {
        public Color Color { get; set; }

        public NarrationErrorData()
        {
            GenerateRandomColor();
        }

        void GenerateRandomColor()
        {
            Color = new Color32(
                (byte) Random.Range(65, 256),
                (byte) Random.Range(50, 176),
                (byte) Random.Range(50, 176),
                255
            );
        }
    }
}