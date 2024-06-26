using UnityEngine;

namespace DS.ScriptableObjects
{
    using Utilities;

    [CreateAssetMenu(menuName = "DialogueSystem/ New Dialogue Event")]
    [System.Serializable]  
    public class DS_DialogueEventSO : ScriptableObject
    {
        public virtual void ExecuteEvent()
        {
            Logger.Message("Event called!");
        }
    }
}
