using System.Collections.Generic;
using UnityEngine;

namespace Variables.Generated
{
    using Variables.Runtime;

    public class VariablesGenerated : MonoBehaviour
    {
        public enum VariablesKey
        {
            INT_1,
			INT_2,
			FLOAT_1,
			FLOAT_2,
			BOOL_1,
			BOOL_2,
			
        }


        public static VariablesGenerated Instance = null;
        public Dictionary<VariablesKey, string> variableMap;

        public Dictionary<string, IntVariable> intVariables;
        public Dictionary<string, FloatVariable> floatVariables;
        public Dictionary<string, BoolVariable> boolVariables;


        private void Awake()
        {
            SetSingleInstance();
        }
        private void SetSingleInstance()
        {
            if (Instance == null) Instance = this;
            else this.enabled = false;
        }
        private void OnEnable()
        {
            InitializeDictionaries();
        }
        private void InitializeDictionaries()
        {
            variableMap = new Dictionary<VariablesKey, string>()
            {
                {VariablesKey.INT_1 , "bea95923-a7dd-4463-96af-70c4d11dc8dd"},
			{VariablesKey.INT_2 , "9a1b779f-6a41-43a8-83bc-b7e42dc17a8b"},
			{VariablesKey.FLOAT_1 , "e3c2ed49-bd94-4a54-a81e-956a14aab867"},
			{VariablesKey.FLOAT_2 , "33bc050d-54f9-4c68-915a-18d8fb569f59"},
			{VariablesKey.BOOL_1 , "00a75a29-6d5f-4174-aedc-6cabe36bbf67"},
			{VariablesKey.BOOL_2 , "ff7625d2-1f8c-4d2f-911f-9c71ab0808e7"},
			
            };
            intVariables = new Dictionary<string, IntVariable>()
            {
                {"bea95923-a7dd-4463-96af-70c4d11dc8dd" , new IntVariable("INT_1", "bea95923-a7dd-4463-96af-70c4d11dc8dd", 1)},
			{"9a1b779f-6a41-43a8-83bc-b7e42dc17a8b" , new IntVariable("INT_2", "9a1b779f-6a41-43a8-83bc-b7e42dc17a8b", 2)},
			
            };
            floatVariables = new Dictionary<string, FloatVariable>()
            {
                {"e3c2ed49-bd94-4a54-a81e-956a14aab867" , new FloatVariable("FLOAT_1", "e3c2ed49-bd94-4a54-a81e-956a14aab867", 1.1f)},
			{"33bc050d-54f9-4c68-915a-18d8fb569f59" , new FloatVariable("FLOAT_2", "33bc050d-54f9-4c68-915a-18d8fb569f59", 1.2f)},
			
            };
            boolVariables = new Dictionary<string, BoolVariable>()
            {
                {"00a75a29-6d5f-4174-aedc-6cabe36bbf67" , new BoolVariable("BOOL_1", "00a75a29-6d5f-4174-aedc-6cabe36bbf67", true)},
			{"ff7625d2-1f8c-4d2f-911f-9c71ab0808e7" , new BoolVariable("BOOL_2", "ff7625d2-1f8c-4d2f-911f-9c71ab0808e7", false)},
			
            };

        }
    }
}