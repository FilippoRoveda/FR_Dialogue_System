using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game
{
    using DS.Runtime.Enumerations;
    using DS.Runtime.Data;
    using DS.Runtime.ScriptableObjects;


    public class ChoiceInterface : MonoBehaviour, IInterface
    {
        [SerializeField] private DialogueChoice holdedChoice;

        [SerializeField] private TMP_Text choiceText;
        [SerializeField] private Button button;

#if UNITY_EDITOR
        [IsVisible(false)]
#endif 
        public UnityEvent<BaseDialogueSO> ChoiceSelected = new UnityEvent<BaseDialogueSO>();

        #region Unity callbacks
        private void OnEnable()
        {
            button = GetComponent<Button>();
            if (button == null) { button = gameObject.AddComponent<Button>(); }

            button.onClick.AddListener(OnChoiceButtonPressed);
            LenguageManager.LenguageChanged.AddListener(OnLenguageChanged);
        }
        private void OnDisable()
        {
            button.onClick.RemoveListener(OnChoiceButtonPressed);
            LenguageManager.LenguageChanged.RemoveListener(OnLenguageChanged);
        }
        #endregion

        public void SetupInterface(DialogueChoice choiceData)
        {
            choiceText.text = choiceData.ChoiceTexts.Find(x => x.LenguageType == LenguageManager.Instance.CurrentLenguage).Data;
            holdedChoice = choiceData;
            EnableButton();
        }
        public void ResetInterface()
        {
            choiceText.text = "";
            holdedChoice = null;
            EnableButton();
        }

        #region Callbacks
        private void OnChoiceButtonPressed()
        {
            DisableButton();
            Debug.Log("Choice button pressed going to " + holdedChoice.NextDialogue.DialogueName);
            ChoiceSelected?.Invoke(holdedChoice.NextDialogue as BaseDialogueSO);
        }
        public void OnLenguageChanged(LenguageType newLenguage)
        {
            if (holdedChoice != null)
            {
                choiceText.text = holdedChoice.ChoiceTexts.Find(x => x.LenguageType == newLenguage).Data;
            }
        }
        #endregion
        private void EnableButton()
        {
            button.interactable = true;
        }
        private void DisableButton()
        {
            button.interactable = false;
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }
    }
}
