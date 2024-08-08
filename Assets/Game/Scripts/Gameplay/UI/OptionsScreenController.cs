using DS.Runtime.Enumerations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class OptionsScreenController : Singleton<OptionsScreenController>
    {
        public bool isScreenOpened = false;

        private bool isLenguageScreenOpened = false;
        [SerializeField] private GameObject menuScreen;
        [SerializeField] private GameObject lenguageScreen;

        [SerializeField] private Transform changeLenguageButtonList;
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private TMP_Text currentLenguageText;


        [SerializeField] private Button optionButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button lenguageScreenButton;
        [SerializeField] private Button quitButton;

        #region Unity Callbacks
        private new void OnEnable()
        {
            base.OnEnable();
            InscribeEvents();
        }
        private void OnDisable()
        {
            UnscribeEvents();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) 
            {
                OnOptionButtonPressed();
            }
        }
        #endregion

        #region Callbacks
        private void OnOptionButtonPressed() 
        {
            isScreenOpened = !isScreenOpened;
            menuScreen.SetActive(isScreenOpened);
        }
        public void OnChangeLenguageButtonPressed() 
        {
            if (isLenguageScreenOpened == true)
            {
                isLenguageScreenOpened = false;
                lenguageScreen.SetActive(false);
            }
            else
            {
                isScreenOpened = true;
                lenguageScreen.SetActive(true);
            }
        }
        private void OnMainMenuButtonPressed() { SceneManager.LoadScene("01_MainMenu"); }
        private void OnQuitButtonPressed() { Application.Quit(); }
        #endregion
        private void SetupChangeLenguageScreen()
        {
            currentLenguageText.text = LenguageManager.Instance.CurrentLenguage.ToString();
            foreach(LenguageType lenguage in (LenguageType[])System.Enum.GetValues(typeof(LenguageType)))
            {
                var obj = Instantiate(buttonPrefab, changeLenguageButtonList);
                var button = obj.GetComponent<Button>();
                obj.GetComponent<TMP_Text>().text = lenguage.ToString();
                button.onClick.AddListener(() => {
                    LenguageManager.Instance.ChangeLenguage(lenguage);
                    currentLenguageText.text = LenguageManager.Instance.CurrentLenguage.ToString();
                });
            }
        }
        private void InscribeEvents() 
        {
            optionButton.onClick.AddListener(OnOptionButtonPressed);
            mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
            lenguageScreenButton.onClick.AddListener(OnChangeLenguageButtonPressed);
            quitButton.onClick.AddListener(OnQuitButtonPressed);
        }
        private void UnscribeEvents() 
        {
            optionButton.onClick.RemoveListener(OnOptionButtonPressed);
            mainMenuButton.onClick.RemoveListener(OnMainMenuButtonPressed);
            lenguageScreenButton.onClick.RemoveListener(OnChangeLenguageButtonPressed);
            quitButton.onClick.RemoveListener(OnQuitButtonPressed);
        }
    }
}
