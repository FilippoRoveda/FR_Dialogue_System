using UnityEngine;

namespace DS.Editor.Events
{   
    /// <summary>
    /// Base temporary class that represent a GameEvent scriptable object, containing only a string
    /// that will need to be handled by a sort of GameEventManager during the game run.
    /// </summary>
    [CreateAssetMenu(menuName = "DialogueSystem/ New Game Event")]
    [System.Serializable]  
    public class GameEventSO : ScriptableObject
    {
        [SerializeField] public string eventExecutionString = "TestEvent";
    }
}
