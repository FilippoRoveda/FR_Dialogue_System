using UnityEngine;

namespace DS.Editor.ScriptableObjects
{   

    [CreateAssetMenu(menuName = "DialogueSystem/ New Dialogue Event")]
    [System.Serializable]  
    public class DS_EventSO : ScriptableObject
    {
        [SerializeField] public string eventName = "TestEvent";
        public virtual void Execute()
        {
            Debug.Log("Evento lanciato");
        }
    }
}
