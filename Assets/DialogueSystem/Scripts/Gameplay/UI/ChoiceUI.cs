using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace DS.Runtime.Gameplay
{
    using Enums;
    using Runtime.Data;
    using Runtime.ScriptableObjects;


    public class ChoiceUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text choiceText;
        [SerializeField] private DS_DialogueChoiceData holdedChoiceData;

        [SerializeField] private Button button;
#if UNITY_EDITOR
        [IsVisible(false)]
#endif 
        public UnityEvent<DS_DialogueSO> ChoiceSelected = new UnityEvent<DS_DialogueSO>();

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

        public void SetupChoiceUI(DS_DialogueChoiceData choiceData)
        {
            choiceText.text = choiceData.ChoiceTexts.Find(x => x.LenguageType == LenguageManager.Instance.CurrentLenguage).Data;
            holdedChoiceData = choiceData;
            EnableButton();
        }
        public void ResetUI()
        {
            choiceText.text = "";
            holdedChoiceData = null;
            EnableButton();
        }

        #region Callbacks
        private void OnChoiceButtonPressed()
        {
            DisableButton();
            Debug.Log("Choice button pressed going to " + holdedChoiceData.NextDialogue.DialogueName);
            ChoiceSelected?.Invoke(holdedChoiceData.NextDialogue);

        }
        private void OnLenguageChanged(DS_LenguageType newLenguage)
        {
            if (holdedChoiceData != null)
            {
                choiceText.text = holdedChoiceData.ChoiceTexts.Find(x => x.LenguageType == newLenguage).Data;
            }
        }

        private void EnableButton()
        {
            button.interactable = true;
        }
        private void DisableButton() 
        {
            button.interactable = false;
        }
        #endregion
    }
}
