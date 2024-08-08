using System.IO;
using UnityEditor;
using UnityEngine;

namespace Variables.Editor
{
    /// <summary>
    /// Compiler class that use VariableDatabase infomations to generate the GeneratedVariables class that store varibles dictionaries for runtime purpose.
    /// </summary>
    public class VariableCompiler
    {
        private const string templatePath = "Assets/DialogueSystem/Variables/Config/Variables.Generated_Template.txt";


        public static void Compile()
        {
            var fileTemplate = File.ReadAllText(templatePath);
            if (fileTemplate == null || fileTemplate == string.Empty)
            {
                Debug.LogError("Invalid template text file for Variable.Generated generation!");
                return;
            }
            var database = AssetDatabase.LoadAssetAtPath<VariablesDatabase>(VariableSystem.GetDatabasePath());
            if(database == null)
            {
                Debug.LogError("Invalid Asset database asset! File not found or wrong path selected.");
                return;
            }

            string enumContent = "";
            string mapDictionaryContent = "";
            string intDictionaryContent = "";
            string floatDictonaryContent = "";
            string boolDictionaryContent = "";

            foreach (var var in database.GetIntegers())
            {
                string enumLine = var.Name.ToUpper();
                enumLine= enumLine.Replace(" ", "_");

                string mapKey = "{" + "VariablesKey." + enumLine + " , " + '"' + var.ID + '"' + "},\n\t\t\t";
                mapDictionaryContent += mapKey;

                enumLine += "," + "\n\t\t\t";
                enumContent += enumLine;

                string intKey = "{" + '"' + var.ID + '"' + " , " + $"new IntVariable(" + '"' + var.Name + '"' + ", " + '"' + var.ID + '"' + ", " + var.Value + ")},\n\t\t\t";
                intDictionaryContent += intKey;
            }
            foreach (var var in database.GetDecimals())
            {
                string _enum = var.Name.ToUpper();
                _enum = _enum.Replace(" ", "_");

                string mapKey = "{" + "VariablesKey." + _enum + " , " +'"' + var.ID + '"' + "},\n\t\t\t";
                mapDictionaryContent += mapKey;

                _enum += "," + "\n\t\t\t";
                enumContent += _enum;

                string floatKey = "{" + '"' + var.ID + '"' + " , " + $"new FloatVariable(" + '"' + var.Name + '"' + ", " + '"' + var.ID + '"' + ", " + var.Value.ToString().Replace(",",".") +'f' + ")},\n\t\t\t";
                floatDictonaryContent += floatKey;
            }
            foreach (var var in database.GetBooleans())
            {
                string _enum = var.Name.ToUpper();
                _enum = _enum.Replace(" ", "_");

                string mapKey = "{" + "VariablesKey." + _enum + " , " + '"' + var.ID + '"' + "},\n\t\t\t";
                mapDictionaryContent += mapKey;

                _enum += "," + "\n\t\t\t";
                enumContent += _enum;

                string boolKey = "{" + '"' + var.ID + '"' + " , " + $"new BoolVariable(" + '"' + var.Name + '"' + ", " + '"' + var.ID + '"' + ", " + var.Value.ToString().ToLower() + ")},\n\t\t\t";
                boolDictionaryContent += boolKey;
            }

            fileTemplate = fileTemplate.Replace("[VAR_KEYS]", enumContent);
            fileTemplate = fileTemplate.Replace("[MAP_KEYPAIRS]", mapDictionaryContent);
            fileTemplate = fileTemplate.Replace("[INT_VAR_KEYPAIRS]", intDictionaryContent);
            fileTemplate = fileTemplate.Replace("[FLOAT_VAR_KEYPAIRS]", floatDictonaryContent);
            fileTemplate = fileTemplate.Replace("[BOOL_VAR_KEYPAIRS]", boolDictionaryContent);

            File.WriteAllText(VariableSystem.GetGeneratedFilePath(), fileTemplate);
        }
        
    }
}
