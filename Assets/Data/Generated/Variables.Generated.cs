using System.Collections.Generic;
using UnityEngine;

namespace Variables.Runtime
{

    public class VariablesGenerated : MonoBehaviour
    {
        public enum VariablesKey
        {
            NEW_INTEGER_VARIABLE_0,
			NEW_INTEGER_VARIABLE_1,
			NEW_INTEGER_VARIABLE_2,
			NEW_FLOAT_VARIABLE_0,
			NEW_FLOAT_VARIABLE_1,
			NEW_FLOAT_VARIABLE_2,
			NEW_BOOLEAN_VARIABLE_0,
			NEW_BOOLEAN_VARIABLE_1,
			NEW_BOOLEAN_VARIABLE_2,
			
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
                {VariablesKey.NEW_INTEGER_VARIABLE_0 , "bea95923-a7dd-4463-96af-70c4d11dc8dd"},
			{VariablesKey.NEW_INTEGER_VARIABLE_1 , "9a1b779f-6a41-43a8-83bc-b7e42dc17a8b"},
			{VariablesKey.NEW_INTEGER_VARIABLE_2 , "96a722bf-c3b8-4bd2-ae48-425f9f4dd816"},
			{VariablesKey.NEW_FLOAT_VARIABLE_0 , "e3c2ed49-bd94-4a54-a81e-956a14aab867"},
			{VariablesKey.NEW_FLOAT_VARIABLE_1 , "33bc050d-54f9-4c68-915a-18d8fb569f59"},
			{VariablesKey.NEW_FLOAT_VARIABLE_2 , "902639dd-fba0-44ef-b5cb-baeb056318f9"},
			{VariablesKey.NEW_BOOLEAN_VARIABLE_0 , "00a75a29-6d5f-4174-aedc-6cabe36bbf67"},
			{VariablesKey.NEW_BOOLEAN_VARIABLE_1 , "ff7625d2-1f8c-4d2f-911f-9c71ab0808e7"},
			{VariablesKey.NEW_BOOLEAN_VARIABLE_2 , "c2f8ff9a-7c83-4bc8-a94a-1d8cd194ce58"},
			
            };
            intVariables = new Dictionary<string, IntVariable>()
            {
                {"bea95923-a7dd-4463-96af-70c4d11dc8dd" , new IntVariable("New Integer Variable 0", "bea95923-a7dd-4463-96af-70c4d11dc8dd", 0)},
			{"9a1b779f-6a41-43a8-83bc-b7e42dc17a8b" , new IntVariable("New Integer Variable 1", "9a1b779f-6a41-43a8-83bc-b7e42dc17a8b", 0)},
			{"96a722bf-c3b8-4bd2-ae48-425f9f4dd816" , new IntVariable("New Integer Variable 2", "96a722bf-c3b8-4bd2-ae48-425f9f4dd816", 0)},
			
            };
            floatVariables = new Dictionary<string, FloatVariable>()
            {
                {"e3c2ed49-bd94-4a54-a81e-956a14aab867" , new FloatVariable("New Float Variable 0", "e3c2ed49-bd94-4a54-a81e-956a14aab867", 0f)},
			{"33bc050d-54f9-4c68-915a-18d8fb569f59" , new FloatVariable("New Float Variable 1", "33bc050d-54f9-4c68-915a-18d8fb569f59", 0f)},
			{"902639dd-fba0-44ef-b5cb-baeb056318f9" , new FloatVariable("New Float Variable 2", "902639dd-fba0-44ef-b5cb-baeb056318f9", 0f)},
			
            };
            boolVariables = new Dictionary<string, BoolVariable>()
            {
                {"00a75a29-6d5f-4174-aedc-6cabe36bbf67" , new BoolVariable("New Boolean Variable 0", "00a75a29-6d5f-4174-aedc-6cabe36bbf67", false)},
			{"ff7625d2-1f8c-4d2f-911f-9c71ab0808e7" , new BoolVariable("New Boolean Variable 1", "ff7625d2-1f8c-4d2f-911f-9c71ab0808e7", false)},
			{"c2f8ff9a-7c83-4bc8-a94a-1d8cd194ce58" , new BoolVariable("New Boolean Variable 2", "c2f8ff9a-7c83-4bc8-a94a-1d8cd194ce58", false)},
			
            };

        }
    }
}