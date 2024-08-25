using System.Collections.Generic;
using UnityEngine;

namespace Variables.Generated
{
    using Variables.Runtime;

    public class VariablesGenerated : MonoBehaviour
    {
        public enum VariablesKey
        {
            GRAVITAS,
			CAESAR_RELATIONSHIP,
			HADRIAN_RELATIONSHIP,
			MARCUSAURELIUS_RELATIONSHIP,
			CAESAR_OFFENDED,
			HADRIAN_OFFENDED,
			MARCUSAURELIUS_OFFENDED,
			
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
                {VariablesKey.GRAVITAS , "e28c3e33-034f-4cf9-9d85-8b04e083d988"},
			{VariablesKey.CAESAR_RELATIONSHIP , "9f27fc6d-6f83-498f-9958-417ede0ad8e7"},
			{VariablesKey.HADRIAN_RELATIONSHIP , "5d4f4d78-2a83-4d7d-bb96-1095f51d060f"},
			{VariablesKey.MARCUSAURELIUS_RELATIONSHIP , "0be5826a-89fa-431c-9b93-b32ff1ac7064"},
			{VariablesKey.CAESAR_OFFENDED , "750a954b-6b18-4b32-9b52-3d00eec0f6dc"},
			{VariablesKey.HADRIAN_OFFENDED , "99360bc4-db2a-4313-a65c-591a94b9e3ae"},
			{VariablesKey.MARCUSAURELIUS_OFFENDED , "cfb845c0-ad7a-43f7-aedf-3d156a0adf24"},
			
            };
            intVariables = new Dictionary<string, IntVariable>()
            {
                {"e28c3e33-034f-4cf9-9d85-8b04e083d988" , new IntVariable("Gravitas", "e28c3e33-034f-4cf9-9d85-8b04e083d988", 0)},
			
            };
            floatVariables = new Dictionary<string, FloatVariable>()
            {
                {"9f27fc6d-6f83-498f-9958-417ede0ad8e7" , new FloatVariable("Caesar_Relationship", "9f27fc6d-6f83-498f-9958-417ede0ad8e7", 0f)},
			{"5d4f4d78-2a83-4d7d-bb96-1095f51d060f" , new FloatVariable("Hadrian_Relationship", "5d4f4d78-2a83-4d7d-bb96-1095f51d060f", 0f)},
			{"0be5826a-89fa-431c-9b93-b32ff1ac7064" , new FloatVariable("MarcusAurelius_Relationship", "0be5826a-89fa-431c-9b93-b32ff1ac7064", 0f)},
			
            };
            boolVariables = new Dictionary<string, BoolVariable>()
            {
                {"750a954b-6b18-4b32-9b52-3d00eec0f6dc" , new BoolVariable("Caesar_Offended", "750a954b-6b18-4b32-9b52-3d00eec0f6dc", false)},
			{"99360bc4-db2a-4313-a65c-591a94b9e3ae" , new BoolVariable("Hadrian_Offended", "99360bc4-db2a-4313-a65c-591a94b9e3ae", false)},
			{"cfb845c0-ad7a-43f7-aedf-3d156a0adf24" , new BoolVariable("MarcusAurelius_Offended", "cfb845c0-ad7a-43f7-aedf-3d156a0adf24", false)},
			
            };

        }
    }
}