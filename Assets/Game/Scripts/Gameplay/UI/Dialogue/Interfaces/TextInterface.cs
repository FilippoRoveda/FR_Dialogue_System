using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
    using DS.Runtime.Enumerations;
    using DS.Runtime.Data;
    using System.Collections;

    [RequireComponent(typeof(AudioSource))]
    public class TextInterface : MonoBehaviour, IInterface
    {
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private List<LenguageData<string>> holdedTexts;


        private AudioSource _audioSource;

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
            _audioSource = GetComponent<AudioSource>();
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
            var textToDisplay = holdedTexts.Find(x => x.LenguageType == LenguageManager.Instance.CurrentLenguage).Data;
            dialogueText.text = textToDisplay;
            dialogueText.maxVisibleCharacters = 0;

            foreach (char letter in textToDisplay)
            {
                if (dialogueSkipped == true)
                {
                    isTyping = false;
                    dialogueSkipped = false;
                    dialogueText.maxVisibleCharacters = textToDisplay.Length;
                    break;
                }
                if(DialogueManager.Instance.TypingSFXActivated == true) 
                {
                    PlayTypingSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                }
              
                dialogueText.maxVisibleCharacters++;

                yield return new WaitForSeconds(DialogueManager.Instance.TextsTypingSpeed);
            }

            isTyping = false;
            yield return null;
        }

        private void PlayTypingSound(int currentDisplayedCharacterCount, char currentDisplayedCharacter)
        {
            if(currentDisplayedCharacterCount % DialogueManager.Instance.TypingSFXFrequency == 0)
            {
                _audioSource.Stop();
                int hashCode = currentDisplayedCharacter.GetHashCode();
                int randomClipIndex = hashCode % DialogueManager.Instance.TypingSFXClips.Length;
                //int randomClipIndex = Random.Range(0, DialogueManager.Instance.TypingSFXClips.Length);

                int minPitch = (int)DialogueManager.Instance.MinPitch * 100;
                int maxPitch = (int)DialogueManager.Instance.MaxPitch * 100;
                int pitchRange = maxPitch - minPitch;
                if (pitchRange != 0) _audioSource.pitch = ((hashCode % pitchRange) + minPitch) / 100.0f;
                else _audioSource.pitch = DialogueManager.Instance.MaxPitch;

                _audioSource.pitch = Random.Range(DialogueManager.Instance.MinPitch, DialogueManager.Instance.MaxPitch);
                _audioSource.PlayOneShot(DialogueManager.Instance.TypingSFXClips[randomClipIndex]);
            }
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
