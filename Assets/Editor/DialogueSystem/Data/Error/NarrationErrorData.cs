using UnityEngine;

namespace Narration.Data.Error
{
    public class NarrationErrorData
    {
        public Color Color { get; set; }

        public NarrationErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
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