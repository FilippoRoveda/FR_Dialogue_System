using System.IO;
using UnityEditor;
using UnityEngine;

namespace Variables.Editor
{
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

                string mapKey = "{" + "VariablesKey." + enumLine + " , " + '"' + var.Id + '"' + "},\n\t\t\t";
                mapDictionaryContent += mapKey;

                enumLine += "," + "\n\t\t\t";
                enumContent += enumLine;

                string intKey = "{" + '"' + var.Id + '"' + " , " + $"new IntVariable(" + '"' + var.Name + '"' + ", " + '"' + var.Id + '"' + ", " + var.Value + ")},\n\t\t\t";
                intDictionaryContent += intKey;
            }
            foreach (var var in database.GetDecimals())
            {
                string _enum = var.Name.ToUpper();
                _enum = _enum.Replace(" ", "_");

                string mapKey = "{" + "VariablesKey." + _enum + " , " +'"' + var.Id + '"' + "},\n\t\t\t";
                mapDictionaryContent += mapKey;

                _enum += "," + "\n\t\t\t";
                enumContent += _enum;

                string floatKey = "{" + '"' + var.Id + '"' + " , " + $"new FloatVariable(" + '"' + var.Name + '"' + ", " + '"' + var.Id + '"' + ", " + var.Value.ToString().Replace(",",".") +'f' + ")},\n\t\t\t";
                floatDictonaryContent += floatKey;
            }
            foreach (var var in database.GetBooleans())
            {
                string _enum = var.Name.ToUpper();
                _enum = _enum.Replace(" ", "_");

                string mapKey = "{" + "VariablesKey." + _enum + " , " + '"' + var.Id + '"' + "},\n\t\t\t";
                mapDictionaryContent += mapKey;

                _enum += "," + "\n\t\t\t";
                enumContent += _enum;

                string boolKey = "{" + '"' + var.Id + '"' + " , " + $"new BoolVariable(" + '"' + var.Name + '"' + ", " + '"' + var.Id + '"' + ", " + var.Value.ToString().ToLower() + ")},\n\t\t\t";
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
