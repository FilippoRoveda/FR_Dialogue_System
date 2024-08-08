using DS.Runtime.Enumerations;
using TMPro;
using UnityEngine;


namespace Game
{
    using Variables.Runtime;
    using Variables.Generated;
    using VariableKey = Variables.Generated.VariablesGenerated.VariablesKey;

    /// <summary>
    /// Interface component for Dialogue variables display.
    /// </summary>
    public class VariableInterface : MonoBehaviour, IInterface
    {
        private string _linkedVarID = null;
        [SerializeField] private VariableKey _linkedVarKey;

        [SerializeField] private TMP_Text _varNameText;
        [SerializeField] private TMP_Text _varValueText;



        #region Unity Callbacks
        private void OnEnable()
        {
            VariableEvents.VariableNameChanged.AddListener(OnVariableNameChanged);
            VariableEvents.VariableValueChanged.AddListener(OnVarValueChanged);
        }
        private void OnDisable()
        {
            VariableEvents.VariableNameChanged.RemoveListener(OnVariableNameChanged);
            VariableEvents.VariableValueChanged.RemoveListener(OnVarValueChanged);
        }
        #endregion


        public void Initialize(VariableKey varKey, string varID)
        {
            _varNameText.text = varKey.ToString().ToUpper().Replace("_"," ");
            _linkedVarID = varID;
            UpdateInterfaceValue();
        }

        #region Callbacks
        public void OnLenguageChanged(LenguageType newLenguage) { }
        private void OnVariableNameChanged(string varID, string newVarName) 
        {
            if(_linkedVarID == varID)
            {
                _varNameText.text = newVarName;
            }
        }
        private void OnVarValueChanged(string varID) 
        {
            if (_linkedVarID == varID)
            {
                UpdateInterfaceValue();
            }
        }
        #endregion

        private void UpdateInterfaceValue()
        {
            if (VariablesGenerated.Instance.intVariables.ContainsKey(_linkedVarID) == true)
            {
                _varValueText.text = VariablesGenerated.Instance.intVariables[_linkedVarID].Value.ToString();
            }
            else if (VariablesGenerated.Instance.floatVariables.ContainsKey(_linkedVarID) == true)
            {
                _varValueText.text = VariablesGenerated.Instance.floatVariables[_linkedVarID].Value.ToString();
            }
            else if (VariablesGenerated.Instance.boolVariables.ContainsKey(_linkedVarID) == true)
            {
                _varValueText.text = VariablesGenerated.Instance.boolVariables[_linkedVarID].Value.ToString();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"No generated variable founded with ID: {_linkedVarID}");
#endif
            }
        }
        public void ResetInterface()
        {
            _linkedVarID = null;
            _linkedVarKey = default;
            _varNameText.text = "";
            _varValueText.text = "";
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
