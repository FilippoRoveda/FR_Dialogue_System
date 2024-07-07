using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVTool : MonoBehaviour
{
    [UnityEditor.MenuItem("DialogueSystem/CSV/Save all graphs to CSV")]
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
        UpdateLenguagesHelper helper = new UpdateLenguagesHelper();
        helper.UpdateLenguages();

        EditorApplication.Beep();
        Debug.Log("<color=green> Update all dialogues lenguages completed! </color>");
    }
}
