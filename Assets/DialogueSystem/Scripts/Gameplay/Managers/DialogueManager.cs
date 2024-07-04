using UnityEngine;
using UnityEngine.Events;

namespace DS.Runtime.Gameplay
{
    using Editor.Utilities;
    using Runtime.ScriptableObjects;
    using Runtime.Utilities;

    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField][IsInteractable(false)] private bool isDialogueRunning = false;

        public static UnityEvent<TalkComponent, DS_DialogueSO> DialogueStarted = new UnityEvent<TalkComponent, DS_DialogueSO>();
        public static UnityEvent<DS_DialogueSO, bool> DialogueEnded = new UnityEvent<DS_DialogueSO, bool>();

        [SerializeField][IsInteractable(false)] private DS_DialogueSO currentStartedDialogue;

        public void StartDialogue(TalkComponent talkComponent, DS_DialogueSO startDialogue)
        {
            if(isDialogueRunning == true)
            {
                DS_Logger.Error("Another dialogue is currently running!");
            }
            else 
            {
                currentStartedDialogue = startDialogue;

                DialogueStarted?.Invoke(talkComponent, startDialogue);
                isDialogueRunning = true;
            }
        }

        public void EndDialogue(DS_DialogueSO endedDialogue, bool isRepeatable)
        {
            if(isDialogueRunning == true && currentStartedDialogue == endedDialogue)
            {
                DialogueEnded?.Invoke(endedDialogue, isRepeatable);
                currentStartedDialogue = null;
                isDialogueRunning = false;
            }
            else
            {
                DS_Logger.Error("No dialogue is currently running or no matching dialogue to end was running in this manager!");
            }
        }
    }
}
