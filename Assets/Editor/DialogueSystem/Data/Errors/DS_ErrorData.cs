using UnityEngine;

namespace DS.Data.Error
{
    [System.Serializable]
    public class DS_ErrorData
    {
        public Color32 ErrorColor { get; private set; }

        public DS_ErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            ErrorColor = new Color32(
                (byte)Random.Range(65, 256),
                (byte)Random.Range(50, 176),
                (byte)Random.Range(50, 176),
                (byte)255
                );
        }
    }
}
