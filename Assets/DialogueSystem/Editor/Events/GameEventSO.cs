using UnityEngine;

namespace DS.Editor.Events
{   
    [CreateAssetMenu(menuName = "DialogueSystem/ New Game Event")]
    [System.Serializable]  
    public class GameEventSO : ScriptableObject
    {
        [SerializeField] public string eventName = "TestEvent";
        public virtual void Execute()
        {
            Debug.Log("Evento lanciato");
        }
    }
}
