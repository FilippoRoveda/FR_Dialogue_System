using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    using DS.Runtime.ScriptableObjects;
    /// <summary>
    /// Handle central dialogue informations and handle and execute launching dialogue requests.
    /// </summary>
    public class DialogueManager : Singleton<DialogueManager>
    {
        [Space]
        [Header("Typing Effects Region")]
        [SerializeField] private float _textsTypingSpeed = 0.08f;
        public float TextsTypingSpeed { get { return _textsTypingSpeed; } }

        [Range(1, 5)]
        [SerializeField] private int _typingSFXFrequency = 2;
        public int TypingSFXFrequency { get { return _typingSFXFrequency; } }

        [Range(-3, 3)]
        [SerializeField] private float _minPitch = 1.0f;
        public float MinPitch { get { return _minPitch; } }
        [Range(-3, 3)]
        [SerializeField] private float _maxPitch = 1.0f;
        public float MaxPitch { get { return _maxPitch; } }

        [SerializeField] private AudioClip[] _audioClips;
        public AudioClip[] TypingSFXClips { get { return _audioClips; } }

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


        public void StartDialogueRequest(TalkComponent dialogueSpeaker, DialogueSO startingDialogue)
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

        public void EndDialogueRequest(DialogueSO endedDialogue, bool isRepeatable)
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
