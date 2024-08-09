using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    using DS.Runtime.Conditions;
    using DS.Runtime.Enumerations;
    using DS.Runtime.Events;
    using DS.Runtime.ScriptableObjects;
    using System.Collections;

    public class DialogueController : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueInterface;

        [SerializeField] private TMP_Text playerName;
        [SerializeField] private Image playerIcon;

        [SerializeField] private TMP_Text speakerName;
        [SerializeField] private Image speakerIcon;


        [SerializeField] private TextInterface dialogueText;
        [SerializeField] private List<ChoiceInterface> choiceInterfaces;

        [SerializeField] private Transform choicesTransform;
        [SerializeField] private GameObject choicePrefab;

        [SerializeField] private Button endButton;


        [SerializeField] private TalkComponent dialogueSpeaker;
        [SerializeField] private BaseDialogueSO startedDialogue;

        [SerializeField] private BaseDialogueSO currentDialogue;

        private ConditionsHandler conditionsHandler;
        private VariableEventsExecuter variableEventsHandler;

        private Coroutine textTypingCoroutine = null;
        private Coroutine interfaceUpdateRoutine = null;

        #region Unity callbacks
        private void Awake()
        {
            Initialize();
        }
        private void OnEnable()
        {
            DialogueManager.DialogueStarted.AddListener(OnDialogueStart); //Listen to manager
            endButton.onClick.AddListener(OnEndDialogueButtonPressed); //Link end button to his callback
        }
        private void OnDisable()
        {
            DialogueManager.DialogueStarted.RemoveListener(OnDialogueStart);
            endButton.onClick.RemoveListener(OnEndDialogueButtonPressed);
        }
        #endregion

        private void Initialize()
        {

            endButton.gameObject.SetActive(false);

            for(int i = 0; i < 5; i++)
            {
                InstantiateChoiceButton();
            }

            dialogueInterface.SetActive(false);

            conditionsHandler = new ConditionsHandler();
            variableEventsHandler = new VariableEventsExecuter();
        }

        private void InstantiateChoiceButton()
        {
            var obj = Instantiate(choicePrefab, choicesTransform);
            var choice = obj.GetComponent<ChoiceInterface>();
            choiceInterfaces.Add(choice);

            choice.ChoiceSelected.AddListener(OnChoiceSelected);

            choice.gameObject.SetActive(false);
        }

        #region Callbacks
        public void OnDialogueStart(TalkComponent talkComponent, DialogueSO startingDialogue) 
        {
            dialogueInterface.SetActive(true);

            dialogueSpeaker = talkComponent;
            startedDialogue = startingDialogue;

            SetupPlayerArea();
            //COROUTINE
            UpdateInterfaceRoutine(startingDialogue);
        }
        public void OnEndDialogueButtonPressed()
        {
            bool isRepetable = false;
            if (currentDialogue.DialogueType == DialogueType.End) isRepetable = ((EndDialogueSO)currentDialogue).IsRepetable;
            DialogueManager.Instance.TryEndDialogue((DialogueSO)startedDialogue, isRepetable);

            ClearFields();
            dialogueInterface.SetActive(false);
        }

        public void OnChoiceSelected(BaseDialogueSO nextDialogue)
        {
            Debug.Log($"Next dialogue pressed: {nextDialogue.DialogueName}");
            UpdateInterfaceRoutine(nextDialogue);
        }

        #endregion
        private void UpdateInterfaceRoutine(BaseDialogueSO dialogue)
        {
            if (interfaceUpdateRoutine != null)
            {
                StopCoroutine(interfaceUpdateRoutine);
            }
            interfaceUpdateRoutine = StartCoroutine(GoNextDialogue(dialogue));
        }

        public IEnumerator GoNextDialogue(BaseDialogueSO dialogue) 
        {
            Debug.Log($"Loading in the coroutine the node{dialogue.DialogueName} of type {dialogue.DialogueType}");
            ClearFields();

            currentDialogue = dialogue;

            switch(dialogue.DialogueType)
            {
                case DialogueType.Branch:
                    BranchDialogueSO branchDialogue = (BranchDialogueSO)currentDialogue;
                    if (conditionsHandler.HandleConditions(branchDialogue.Condtitions) == true)
                    {
                        UpdateInterfaceRoutine(branchDialogue.Choices[0].NextDialogue);
                    }
                    else
                    {
                        UpdateInterfaceRoutine(branchDialogue.Choices[1].NextDialogue);
                    }
                    yield break;

                case DialogueType.Event:
                    EventDialogueSO eventDialogue = (EventDialogueSO)currentDialogue;
                    foreach (var gameEvent in eventDialogue.Events)
                    {
                        Debug.Log("To implement event call");
                    }
                    variableEventsHandler.ExecuteEvents(eventDialogue.VariableEvents);
                    break;
            }


            SetupSpeakerArea();
            SetupDialogueText();

            if(textTypingCoroutine != null)
            {
                StopCoroutine(textTypingCoroutine);
                dialogueText.StopTyping();
            }

            textTypingCoroutine = StartCoroutine(dialogueText.GetDiaplayTextRoutine());
            yield return textTypingCoroutine;

            if(currentDialogue.DialogueType != DialogueType.End) SetupChoices((DialogueSO)currentDialogue);
            else if(currentDialogue.DialogueType == DialogueType.End) endButton.gameObject.SetActive(true);
        }
        public void ClearFields()
        {
            endButton.gameObject.SetActive(false);
            
            foreach (var choice in choiceInterfaces)
            {
                choice.ResetInterface();
            }

            dialogueText.ResetInterface();

            for (int i = 0; i < choiceInterfaces.Count; i++)
            {
                choiceInterfaces[i].gameObject.SetActive(false);
            }
        }
        public void SetupPlayerArea()
        {
            playerName.text = PlayerComponent.Instance.Data.CompleteName;
            playerIcon.sprite = PlayerComponent.Instance.Data.Icon;
        }
        public void SetupSpeakerArea() 
        {
            speakerName.text = dialogueSpeaker.LinkedCharacter.Data.CompleteName;
            speakerIcon.sprite = dialogueSpeaker.LinkedCharacter.Data.Icon;
        }

        public void SetupDialogueText() 
        {
            dialogueText.SetupInterface((currentDialogue as TextedDialogueSO).Texts);
        }
        public void SetupChoices(DialogueSO dialogue)
        {
            if (dialogue.Choices == null | dialogue.Choices.Count == 0)
            {
                endButton.gameObject.SetActive(true);
                return;
            }
            for (int i = 0; i < dialogue.Choices.Count; i++)
            {
                if (conditionsHandler.HandleConditions(dialogue.Choices[i].Conditions) == true)
                {
                    choiceInterfaces[i].SetupInterface(dialogue.Choices[i]);
                    choiceInterfaces[i].gameObject.SetActive(true);
                }
                else continue;
            }
        }    
    }
}
