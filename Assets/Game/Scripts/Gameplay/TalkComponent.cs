using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    using DS.Runtime.ScriptableObjects;

    public class TalkComponent : MonoBehaviour
    {
        [SerializeField] private List<DialogueContainerSO> dialogueContainers;

        [SerializeField] private Charcter _linkedCharacter;
        public Charcter LinkedCharacter { get { return _linkedCharacter; } }

        [SerializeField] private TalkZone _talkZone;

        [SerializeField] List<BaseDialogueSO> availableDialogues;
        [SerializeField] BaseDialogueSO ongoingDialogue;


        #region Unity callbacks
        private void Awake()
        {
            if (dialogueContainers == null || dialogueContainers.Count == 0)
            {
#if UNITY_EDITOR
                Debug.LogWarning("No dialogues data for this TalkComponent.");
                return;
#endif
            }
        }

        private void OnEnable()
        {
            _talkZone = GetComponent<TalkZone>();
            if (_talkZone == null)
            {
                _talkZone = gameObject.AddComponent<TalkZone>();
            }
        }
        #endregion

        public void Initialize(Charcter character)
        {
            availableDialogues = new List<BaseDialogueSO>();
            LoadAvailableDialogues();
            _linkedCharacter = character;

            _talkZone.TalkButtonPressed.AddListener(OnTalkButtonPressed);
        }

        private void LoadAvailableDialogues()
        {
            foreach (var container in dialogueContainers)
            {
                var startingDialogues = container.GetStartingDialogues();

                foreach (DialogueSO _dialogue in startingDialogues)
                {
                    availableDialogues.Add(_dialogue);
                }
            }
        }

        #region Callbacks
        private void OnTalkButtonPressed()
        {
            DisableTalks();

            var nextDialogue = availableDialogues[0];
            ongoingDialogue = nextDialogue;

    #if UNITY_EDITOR
                Debug.LogError("Starting dialogue: " + nextDialogue.DialogueName);
    #endif
            DialogueManager.DialogueEnded.AddListener(OnDialogueEnded);
            DialogueManager.Instance.StartDialogue(this, (DialogueSO)nextDialogue);
        }

        private void OnDialogueEnded(DialogueSO endedDialogue, bool couldBeRepeated)
        {
            Debug.Log("Dialogue Ending");
            if (ongoingDialogue == endedDialogue)
            {
                if (couldBeRepeated == false) availableDialogues.Remove(ongoingDialogue);
                else
                {
                    availableDialogues.Remove(ongoingDialogue);
                    availableDialogues.Add(ongoingDialogue);
                }
                //currentSpokeDialogue = default;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError("Current ongoing dialogue for this talk component does not match the current ending dialogue.");
#endif            
            }
            DialogueManager.DialogueEnded.RemoveListener(OnDialogueEnded);

            EnableTalks();
        }
        #endregion
        #region Utilities
        public void EnableTalks()
        {
            _talkZone.EnableZone();
        }
        public void DisableTalks()
        {
            _talkZone.DisableZone();
        }

        public void AddAvailableDialogue(DialogueContainerSO dialogue)
        {
            if (dialogueContainers.Contains(dialogue) == false)
            {
                dialogueContainers.Add(dialogue);
            }
#if UNITY_EDITOR
            Debug.LogWarning("Trying to add an already contained dialogue to this TalkComponent");
#endif
        }
        public void RemoveAvailableDialogue(DialogueContainerSO dialogue)
        {
            if (dialogueContainers.Contains(dialogue) == true)
            {
                dialogueContainers.Remove(dialogue);
            }
#if UNITY_EDITOR
            Debug.LogWarning("Trying to remove a non contained dialogue from this TalkComponent");
#endif
        }
    }
    #endregion
}
