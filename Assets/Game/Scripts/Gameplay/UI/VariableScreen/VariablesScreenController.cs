using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Variables.Generated;

namespace Game
{
    public class VariablesScreenController : Singleton<VariablesScreenController>
    {
        private float animationDuration = 1.0f;

        [SerializeField] private Transform variableListTransform;
        [SerializeField] private GameObject variableInterfacePrefab;

        [SerializeField] private RectTransform screenInterface;

        [SerializeField] private Button openScreenButton;
        [SerializeField] private Button closeScreenButton;

        private bool isScreenOpened = false;
        private bool isAnimating = false;

        #region Unity Callbacks
        private new void OnEnable()
        {
            base.OnEnable();
            openScreenButton.onClick.AddListener(OnOpenScreenPressed);
            closeScreenButton.onClick.AddListener(OnCloseScreenPressed);
        }
        private void OnDisable()
        {
            ClearVariableList();
            openScreenButton.onClick.RemoveListener(OnOpenScreenPressed);
            closeScreenButton.onClick.RemoveListener(OnCloseScreenPressed);
        }

        private void Start()
        {
            InitializeVariables();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) == true && isAnimating == false)
            {
                StartCoroutine(SlideAnimationRoutine());
            }
            if (Input.GetKeyDown(KeyCode.Escape) && OptionsScreenController.Instance.isScreenOpened == false && isScreenOpened == true)
            {
                OnCloseScreenPressed();
            }
        }
        private void OnDestroy()
        {
            ClearVariableList();
        }
        #endregion
        #region Callbacks
        private void OnOpenScreenPressed()
        {
            if (isAnimating == false && isScreenOpened == false)
            {
                StartCoroutine(SlideAnimationRoutine());
            }
        }
        public void OnCloseScreenPressed() 
        {
            if (isAnimating == false && isScreenOpened == true)
            {
                StartCoroutine(SlideAnimationRoutine());
            }
        }
        #endregion
        private IEnumerator SlideAnimationRoutine()
        {
            isAnimating = true;
            openScreenButton.interactable = false;
            closeScreenButton.interactable = false;

            Vector2 startPos = screenInterface.anchoredPosition;
            Vector2 endPos = startPos;

            if (isScreenOpened == true) { endPos.x -= 305; }
            else { endPos.x += 305; }
            isScreenOpened = !isScreenOpened;

            float percent = 0.0f;
            float timer = 0.0f;
            while (timer < animationDuration)
            {
                timer += Time.deltaTime;
                percent = timer / animationDuration;
                screenInterface.anchoredPosition = Vector2.Lerp(startPos, endPos, percent);
                yield return null;
            }
            screenInterface.anchoredPosition = endPos;

            if (isScreenOpened == true) { openScreenButton.interactable = false; closeScreenButton.interactable = true; }
            else if (isScreenOpened == false) {openScreenButton.interactable = true; closeScreenButton.interactable = false; }
            isAnimating = false;
        }

        private void InitializeVariables() 
        {
            foreach (var pair in VariablesGenerated.Instance.variableMap)
            {
                var go = GameObject.Instantiate(variableInterfacePrefab, variableListTransform);
                var _interface = go.GetComponent<VariableInterface>();
                _interface.Initialize(pair.Key, pair.Value);
            }
        }

        private void ClearVariableList()
        {
           for(int i = 2; i < variableListTransform.childCount; i++)
            {
                Destroy(variableListTransform.GetChild(i).gameObject);
            }
        }
    }
}
