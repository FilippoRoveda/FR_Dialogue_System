using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DS.Runtime.Gameplay
{
    using DS.Enums;
    using Runtime.Data;

    public class DialogueTextUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private List<LenguageData<string>> holdedTexts;

        #region Unity callbacks
        private void Awake()
        {
            dialogueText = GetComponentInChildren<TMP_Text>();
            if (dialogueText == null) { dialogueText = gameObject.AddComponent<TMP_Text>(); }
        }
        private void OnEnable()
        {
            LenguageManager.LenguageChanged.AddListener(OnLenguageChanged);
        }
        private void OnDisable()
        {
            LenguageManager.LenguageChanged.RemoveListener(OnLenguageChanged);
        }
        #endregion

        public void SetupTextUI(List<LenguageData<string>> texts)
        {
            dialogueText.text = texts.Find(x => x.LenguageType == LenguageManager.Instance.CurrentLenguage).Data;
            holdedTexts = texts;
        }
        public void ResetUI()
        {
            dialogueText.text = "";
            holdedTexts = null;
        }
        #region Callbacks
        private void OnLenguageChanged(DS_LenguageType newLenguage)
        {
            if (holdedTexts != null)
            {
                dialogueText.text = holdedTexts.Find(x => x.LenguageType == newLenguage).Data;
            }
        }
        #endregion
    }
}
