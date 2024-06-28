using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{   

    [CreateAssetMenu(menuName = "DialogueSystem/ New Dialogue Event")]
    [System.Serializable]  
    public class DS_DialogueEventSO : ScriptableObject
    {
        public virtual void ExecuteEvent()
        {
            
        }
    }
}
