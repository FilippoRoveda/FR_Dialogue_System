using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Runtime.Data;
    using Editor.Elements;

    [System.Serializable]
    public class TextedNodeData : BaseNodeData
    {
        [SerializeField] protected List<LenguageData<string>> texts;
        public List<LenguageData<string>> Texts
        {
            get
            {
                return texts;
            }
            set
            {
                texts = value;
            }
        }

        public TextedNodeData()
        {
            texts = LenguageUtilities.InitLenguageDataSet<string>();
            texts = LenguageUtilities.UpdateLenguageDataSet(texts);
        }


        public TextedNodeData(TextedNode node) : base(node)
        {
            Texts = new List<LenguageData<string>>(node.Data.Texts);
            Texts = LenguageUtilities.UpdateLenguageDataSet(Texts);
        }

        public void UpdateTextsLenguage()
        {
            Texts = LenguageUtilities.UpdateLenguageDataSet(Texts);
        }
    }
}

