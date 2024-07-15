using UnityEngine;

namespace DS.Runtime.ScriptableObjects
{   

    [CreateAssetMenu(menuName = "DialogueSystem/ New Dialogue Event")]
    [System.Serializable]  
    public class DS_EventSO : ScriptableObject
    {
        public virtual void Execute()
        {
            Debug.Log("Evento lanciato");
        }
    }
}
