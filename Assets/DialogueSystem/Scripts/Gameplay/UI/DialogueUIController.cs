using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DS.Runtime.Gameplay
{
    using Runtime.ScriptableObjects;

    public class DialogueUIController : MonoBehaviour
    {
        [SerializeField] private GameObject container;

        [SerializeField] private TMP_Text playerName;
        [SerializeField] private Image playerIcon;

        [SerializeField] private TMP_Text speakerName;
        [SerializeField] private Image speakerIcon;

        [SerializeField] private DialogueTextUI dialogueText;

        [SerializeField] private List<ChoiceUI> choicesUI;

        [SerializeField] private Transform choiceList;
        [SerializeField] private GameObject choiceUIPrefab;

        [SerializeField] private Button endDialogueButton;


        [SerializeField] private TalkComponent startSpeaker;
        [SerializeField] private TextedDialogueSO startingDialogue;

        [SerializeField] private TextedDialogueSO currentDialogue;

        #region Unity callbacks
        private void Awake()
        {
            Initialize();
        }
        private void OnEnable()
        {
            DialogueManager.DialogueStarted.AddListener(OnDialogueStart);
            endDialogueButton.onClick.AddListener(OnEndDialogueButtonPressed);
        }
        private void OnDisable()
        {
            DialogueManager.DialogueStarted.RemoveListener(OnDialogueStart);
            endDialogueButton.onClick.RemoveListener(OnEndDialogueButtonPressed);
        }
        #endregion

        private void Initialize()
        {

            endDialogueButton.gameObject.SetActive(false);

            for(int i = 0; i < 5; i++)
            {
                var obj = GameObject.Instantiate(choiceUIPrefab, choiceList);
                var choice = obj.GetComponent<ChoiceUI>();
                choicesUI.Add(choice);

                choice.ChoiceSelected.AddListener(OnChoiceSelected);

                choice.gameObject.SetActive(false);
            }
            container.SetActive(false);
        }

        #region Callbacks
        public void OnDialogueStart(TalkComponent talkComponent, DialogueSO startingDialogue) 
        {
            container.SetActive(true);

            startSpeaker = talkComponent;
            this.startingDialogue = startingDialogue;

            SetupPlayerArea();
            SetupDialogue(startingDialogue);
        }
        public void OnEndDialogueButtonPressed()
        {
            //bool isRepetable = false;
            //if (currentDialogue.DialogueType == Enums.DialogueType.End) isRepetable = ((EndDialogueSO)currentDialogue).IsDialogueRepetable;
            //DialogueManager.Instance.EndDialogue(startingDialogue, isRepetable);

            //ClearFields();
            //container.SetActive(false);
        }

        public void OnChoiceSelected(DialogueSO nextDialogue)
        {
            Debug.Log($"Next dialogue pressed: {nextDialogue.DialogueName}");
            SetupDialogue(nextDialogue);
        }

        #endregion


        public void SetupDialogue(DialogueSO dialogue) 
        {
            currentDialogue = dialogue;

            if(dialogue.DialogueType == Enums.DialogueType.Event)
            {
                foreach(var _event in ((EventDialogueSO)dialogue).Events)
                {
                    _event.Execute();
                }
            }
            ClearFields();
            SetupSpeakerArea();
            SetupDialogueText();
            SetupChoices();
        }
        public void ClearFields()
        {
            endDialogueButton.gameObject.SetActive(false);
            
            foreach (var choice in choicesUI)
            {
                choice.ResetUI();
            }

            dialogueText.ResetUI();

            for (int i = 0; i < 5; i++)
            {
                choicesUI[i].gameObject.SetActive(false);
            }
        }
        public void SetupPlayerArea()
        {
            playerName.text = Player.Instance.PlayerData.CompleteName;
            playerIcon.sprite = Player.Instance.PlayerData.Icon;
        }
        public void SetupSpeakerArea() 
        {
            speakerName.text = startSpeaker.Charcter.Data.CompleteName;
            speakerIcon.sprite = startSpeaker.Charcter.Data.Icon;
        }
        public void SetupDialogueText() 
        {
            dialogueText.SetupTextUI(currentDialogue.Texts);
        }
        public void SetupChoices()
        {
            //if(currentDialogue.DialogueType == Enums.DialogueType.End | currentDialogue.Choices == null | currentDialogue.Choices.Count == 0)
            //{
            //    endDialogueButton.gameObject.SetActive(true);
            //    return;
            //}
            //for(int i = 0; i < currentDialogue.Choices.Count; i++)
            //{
            //    choicesUI[i].SetupChoiceUI(currentDialogue.Choices[i]);
            //    choicesUI[i].gameObject.SetActive(true);
            //}
        }    
    }
}
