using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Editor.Elements;

    [System.Serializable]
    public class TextedNodeData : BaseNodeData
    {
        [SerializeField] protected List<LenguageData<string>> texts;
        public List<LenguageData<string>> Texts
        {
            get => texts;
            set => texts = value;
        }

        public TextedNodeData()
        {
            texts = LenguageUtilities.InitLenguageDataSet<string>();
            texts = LenguageUtilities.UpdateLenguageDataSet(texts);
        }

        public TextedNodeData(TextedNodeData data) : base(data)
        {
            texts = new List<LenguageData<string>>(data.Texts);
            texts = LenguageUtilities.UpdateLenguageDataSet(Texts);
        }
        public TextedNodeData(TextedNode node) : base(node)
        {
            texts = new List<LenguageData<string>>(node._texts);
            texts = LenguageUtilities.UpdateLenguageDataSet(Texts);
        }

        public void UpdateTextsLenguage()
        {
            texts = LenguageUtilities.UpdateLenguageDataSet(texts);
        }
    }
}

