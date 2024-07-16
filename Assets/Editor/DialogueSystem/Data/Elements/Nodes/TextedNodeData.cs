using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Editor.Data
{
    using Runtime.Data;
    using Enums;
    using UnityEditor.TestTools.TestRunner.Api;

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

        public TextedNodeData(string _nodeID, string _dialogueName, List<LenguageData<string>> _texts, DialogueType _dialogueType,
                                string _groupID, Vector2 _position) : base(_nodeID, _dialogueName, _dialogueType, _groupID, _position)
        {         

            this.Texts = new List<LenguageData<string>>(_texts);
            this.Texts = LenguageUtilities.UpdateLenguageDataSet(_texts);
        }

        public void UpdateTextsLenguage()
        {
            Texts = LenguageUtilities.UpdateLenguageDataSet(Texts);
        }
    }
}

