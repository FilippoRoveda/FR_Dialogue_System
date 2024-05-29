using UnityEngine;


namespace DS.Data
{
    using ScriptableObjects;

    [System.Serializable]
    public class DS_DialogueChoiceData
    {
        [SerializeField] public string Text { get; set; }
        [SerializeField] public DS_Dialogue_SO NextDialogue { get; set; }
    }
}
