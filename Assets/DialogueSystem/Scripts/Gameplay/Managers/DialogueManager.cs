using UnityEngine;
using UnityEngine.Events;

namespace DS.Runtime.Gameplay
{
    using Runtime.ScriptableObjects;
    using Runtime.Utilities;

    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField]
#if UNITY_EDITOR
        [IsInteractable(false)]
#endif
        private bool isDialogueRunning = false;

        public static UnityEvent<TalkComponent, DialogueSO> DialogueStarted = new UnityEvent<TalkComponent, DialogueSO>();
        public static UnityEvent<DialogueSO, bool> DialogueEnded = new UnityEvent<DialogueSO, bool>();

        [SerializeField]
#if UNITY_EDITOR
        [IsInteractable(false)]
#endif
        private DialogueSO currentStartedDialogue;

        public void StartDialogue(TalkComponent talkComponent, DialogueSO startDialogue)
        {
            if(isDialogueRunning == true)
            {
#if UNITY_EDITOR
                Debug.LogError("Another dialogue is currently running!");
#endif
            }
            else 
            {
                currentStartedDialogue = startDialogue;

                DialogueStarted?.Invoke(talkComponent, startDialogue);
                isDialogueRunning = true;
            }
        }

        public void EndDialogue(DialogueSO endedDialogue, bool isRepeatable)
        {
            if(isDialogueRunning == true && currentStartedDialogue == endedDialogue)
            {
                DialogueEnded?.Invoke(endedDialogue, isRepeatable);
                currentStartedDialogue = null;
                isDialogueRunning = false;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("No dialogue is currently running or no matching dialogue to end was running in this manager!");
#endif
            }
        }
    }
}
