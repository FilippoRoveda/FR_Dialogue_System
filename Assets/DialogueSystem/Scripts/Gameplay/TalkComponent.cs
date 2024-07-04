using System.Collections.Generic;
using UnityEngine;


namespace DS.Runtime.Gameplay
{
    using Runtime.ScriptableObjects;
    using Editor.Utilities;

    public class TalkComponent : MonoBehaviour
    {
        [SerializeField] private List<DS_DialogueContainerSO> dialogues;

        [SerializeField] private Charcter _charcter;
        public Charcter Charcter { get { return _charcter; } }

        [SerializeField] private TalkZone _talkZone;

        [SerializeField] List<DS_DialogueSO> availableDialogues = new();

        [SerializeField] DS_DialogueSO currentSpokeDialogue;

        #region Unity callbacks
        private void Awake()
        {
            if (dialogues == null || dialogues.Count == 0)
            {
                Debug.LogError("No dialogues data for this TalkComponent.");
                return;
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
            SetupAvailableDialogues();
            _charcter = character;
            _talkZone.TalkButtonPressed.AddListener(LaunchDialogue);
        }

        private void SetupAvailableDialogues()
        {
            foreach (var dialogue in dialogues)
            {
                var startingDialogues = dialogue.GetStartingDialogues();

                foreach (DS_DialogueSO _dialogue in startingDialogues)
                {
                    Debug.Log(_dialogue.DialogueName);

                    availableDialogues.Add(_dialogue);
                    Debug.Log(availableDialogues.Count);
                }
            }
        }

        private void LaunchDialogue()
        {
            DisableTalks();

            var nextDialogue = availableDialogues[0];
            currentSpokeDialogue = nextDialogue;

            DS_Logger.Message("Starting dialogue: " + nextDialogue.DialogueName);

            DialogueManager.DialogueEnded.AddListener(OnDialogueEnded);
            DialogueManager.Instance.StartDialogue(this, nextDialogue);
        }

        private void OnDialogueEnded(DS_DialogueSO endedDialogue, bool couldBeRepeated)
        {
            Debug.Log("dIALOGYE ENDING");
            if (currentSpokeDialogue == endedDialogue)
            {
                if (couldBeRepeated == false) availableDialogues.Remove(currentSpokeDialogue);
                else
                {
                    availableDialogues.Remove(currentSpokeDialogue);
                    availableDialogues.Add(currentSpokeDialogue);
                }
                currentSpokeDialogue = default;
            }
            else DS_Logger.Error("Current spoke dialogue does not match the ending dialogue.", Color.red);

            DialogueManager.DialogueEnded.RemoveListener(OnDialogueEnded);

            EnableTalks();
        }
        public void EnableTalks()
        {
            _talkZone.EnableZone();
        }
        public void DisableTalks()
        {
            _talkZone.DisableZone();
        }
    }
}
