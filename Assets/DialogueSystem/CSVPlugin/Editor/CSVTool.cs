using UnityEditor;
using UnityEngine;

namespace CSVPlugin
{
    public class CSVTool : MonoBehaviour
    {
        public static readonly string CSVFilesPath = "Assets/Editor/Data/CSV/";
        public static readonly string graphFilesPath = "Assets/Editor/Data/Graphs";

        public static readonly string generateDataFolderName = "Data";
        public static readonly string generatedCSVFolderName = "CSV";
        public static readonly string generatedGraphsFolderName = "Graphs";


        [MenuItem("DialogueSystem/CSV/Save all graphs to CSV")]
        public static void SaveGraphsToCSV()
        {
            SaveCSV saveCSV = new SaveCSV();
            saveCSV.Initalize();
            saveCSV.SaveAllGraphsToCSV();

            EditorApplication.Beep();
            Debug.Log("<color=green> All graph had been saved to CSV files! </color>");
        }

        [UnityEditor.MenuItem("DialogueSystem/CSV/Load all CSV in Graphs")]
        public static void LoadCSVInGraphs()
        {
            LoadCSV loadCSV = new LoadCSV();
            bool errorFlag = loadCSV.LoadAllCSVInToGraphs();

            if (errorFlag)
            {
                EditorApplication.Beep();
                EditorApplication.Beep();
                Debug.Log("<color=ref> All CSV file had been loaded to all DS_GraphSO objects but some problems had happened during the loading phase. </color>");
            }
            else
            {
                EditorApplication.Beep();
                Debug.Log("<color=green> All CSV file had been loaded to all DS_GraphSO objects! Every Graph is now updated. </color>");
            }
        }
        [UnityEditor.MenuItem("DialogueSystem/Update all dialogue Lenguages")]
        public static void UpdateLenguages()
        {
            CSVLenguageHelper helper = new CSVLenguageHelper();
            helper.UpdateLenguages();

            EditorApplication.Beep();
            Debug.Log("<color=green> Update all dialogues lenguages completed! </color>");
        }
    }
}

