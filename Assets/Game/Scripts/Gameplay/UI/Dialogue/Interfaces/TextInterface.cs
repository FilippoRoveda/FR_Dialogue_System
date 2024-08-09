using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
    using DS.Runtime.Enumerations;
    using DS.Runtime.Data;
    using System.Collections;

    public class TextInterface : MonoBehaviour, IInterface
    {
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private List<LenguageData<string>> holdedTexts;

        private bool isTyping = false;
        private bool dialogueSkipped = false;

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
        private void Update()
        {
            if (isTyping == true && Input.GetKeyDown(KeyCode.Space))
            {
                dialogueSkipped = true;
            }
        }
        #endregion

        public void SetupInterface(List<LenguageData<string>> texts)
        {
            holdedTexts = texts;
        }

        public IEnumerator GetDiaplayTextRoutine()
        {
            if (isTyping == false)
            {
                return DisplayTextCoroutine();
            }
            else return null;
        }
        public void StopTyping() { isTyping = false; }
        private IEnumerator DisplayTextCoroutine()
        {
            isTyping = true;
            dialogueText.text = "";

            var textToDisplay = holdedTexts.Find(x => x.LenguageType == LenguageManager.Instance.CurrentLenguage).Data;
            
            foreach (char letter in textToDisplay)
            {
                if (dialogueSkipped == true)
                {
                    isTyping = false;
                    dialogueSkipped = false;
                    dialogueText.text = textToDisplay;
                    break;
                }
                dialogueText.text += letter;
                yield return new WaitForSeconds(DialogueManager.Instance.TextsTypingSpeed);
            }

            isTyping = false;
            yield return null;
        }
        public void ResetInterface()
        {
            dialogueText.text = "";
            holdedTexts = null;
        }
        #region Callbacks
        public void OnLenguageChanged(LenguageType newLenguage)
        {
            if (holdedTexts != null)
            {
                dialogueText.text = holdedTexts.Find(x => x.LenguageType == newLenguage).Data;
            }
        }

        public void Show()
        {
            throw new System.NotImplementedException();
        }

        public void Hide()
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
