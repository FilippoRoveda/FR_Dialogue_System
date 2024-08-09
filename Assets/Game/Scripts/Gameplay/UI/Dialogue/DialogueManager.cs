using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    using DS.Runtime.ScriptableObjects;

    public class DialogueManager : Singleton<DialogueManager>
    {
        [SerializeField] private float textsTypingSpeed = 0.08f;
        public float TextsTypingSpeed { get { return textsTypingSpeed; } }
#if UNITY_EDITOR
        [IsInteractable(false)]
#endif
        [SerializeField] private bool isDialogueRunning = false;

        
#if UNITY_EDITOR
        [IsInteractable(false)]
#endif
        [SerializeField] private DialogueSO currentDialogue;


        public static UnityEvent<TalkComponent, DialogueSO> DialogueStarted = new UnityEvent<TalkComponent, DialogueSO>();

        public static UnityEvent<string, bool> DialogueEnded = new UnityEvent<string, bool>();


        public void TryStartDialogue(TalkComponent dialogueSpeaker, DialogueSO startingDialogue)
        {
            if(isDialogueRunning == true)
            {
#if UNITY_EDITOR
                Debug.LogError("Another dialogue is currently running in this scene!");
#endif
                return;
            }

            currentDialogue = startingDialogue;
            DialogueStarted?.Invoke(dialogueSpeaker, startingDialogue);
            isDialogueRunning = true;
        }

        public void TryEndDialogue(DialogueSO endedDialogue, bool isRepeatable)
        {
            if(isDialogueRunning == true && currentDialogue == endedDialogue)
            {
                DialogueEnded?.Invoke(endedDialogue.DialogueID, isRepeatable);
                currentDialogue = null;
                isDialogueRunning = false;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("No dialogue is currently running or no matching ending dialogue was running in this manager!");
#endif
            }
        }
    }
}
