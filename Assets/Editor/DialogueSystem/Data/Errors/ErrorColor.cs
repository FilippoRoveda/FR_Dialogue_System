using UnityEngine;

namespace DS.Data.Error
{
    /// <summary>
    /// Class that generate a random color for error purpose.
    /// </summary>
    [System.Serializable]
    public class ErrorColor
    {
        public Color32 Color { get; private set; }

        public ErrorColor()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = new Color32(
                (byte)Random.Range(65, 256),
                (byte)Random.Range(50, 176),
                (byte)Random.Range(50, 176),
                (byte)255
                );
        }
    }
}
