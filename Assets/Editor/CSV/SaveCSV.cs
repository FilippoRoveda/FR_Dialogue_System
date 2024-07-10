using DS.Editor.ScriptableObjects;
using DS.Editor.Windows.Utilities;
using DS.Enums;
using DS.Runtime.Data;
using System;
using System.Collections.Generic;
using System.IO;

public class SaveCSV
{
    DS_IOUtilities IO = new();
    private readonly string CSVFilesPath = "Assets/Editor/Files/CSV/";
    private readonly string graphFilesPath = "Assets/Editor/Files/Graphs";

    private string graphName = "";
    private readonly string csvExtension = ".csv";
    string FileName { get { return graphName + csvExtension; } }

    private string csvSeparator = ",";
    private List<string> csvHeaders;
    private string idName = "Guid ID";
    private string dialogueName = "Dialogue/Choice Name";

    public SaveCSV() { CreateStaticFolders(); }

    public void Initalize()
    {
        SetHeaders();
    }

    public void SaveAllGraphsToCSV() 
    {
        List<DS_GraphSO> graphs = IO.LoadAssetsFromPath<DS_GraphSO>(graphFilesPath);

        foreach (var graph in graphs)
        {
            graphName = graph.GraphName;
            CreateFile(FileName);

            foreach (var nodeData in graph.GetAllOrderedNodes())
            {
                List<string> nodeTexts = new List<string>();
                nodeTexts.Add(nodeData.NodeID);
                nodeTexts.Add(nodeData.Name);
                foreach(DS_LenguageType lenguage in (DS_LenguageType[])Enum.GetValues(typeof(DS_LenguageType)))
                {
                    string lenguageText = nodeData.Texts.GetLenguageData(lenguage).Data.Replace("\"", "\"\"");
                    nodeTexts.Add($"\"{lenguageText}\"");
                }
                AppendToFile(nodeTexts);


                if (nodeData.Choices != null && nodeData.Choices.Count != 0)
                {
                    int counter = 1;
                    foreach (var choice in nodeData.Choices)
                    {
                        List<string> nodeChoiceTexts = new List<string>();
                        nodeChoiceTexts.Add(choice.ChoiceID);
                        nodeChoiceTexts.Add($"_____{nodeData.Name} Choice [{counter}]");
                        counter++;
                        foreach (DS_LenguageType lenguage in (DS_LenguageType[])Enum.GetValues(typeof(DS_LenguageType)))
                        {
                            string choiceText = choice.ChoiceTexts.GetLenguageData(lenguage).Data.Replace("\"", "'\"\"'");
                            nodeChoiceTexts.Add($"\"{choiceText}\"");
                        }
                        AppendToFile(nodeChoiceTexts);
                    }
                }
            }
        }
    }

    public void CreateFile(string fileName) 
    {
        string headerString = GetHeaderString();
        if (File.Exists(CSVFilesPath + fileName))
        {
            File.WriteAllText(CSVFilesPath + fileName, headerString);
        }
        else
        {
            using (StreamWriter sw = File.CreateText(CSVFilesPath + fileName))
            {
                
                sw.WriteLine(headerString);
            }
        }
    }

    private string GetHeaderString()
    {
        string headerString = "";
        foreach (string header in csvHeaders)
        {
            if (headerString != "") headerString += csvSeparator;
            headerString += header;
        }
        headerString += "\n";
        return headerString;
    }

    public void AppendToFile(List<string> strings)
    {
        using (StreamWriter sw = File.AppendText(CSVFilesPath + FileName))
        {
            string finalString = "";
            foreach (string _string in strings)
            {
                if (finalString != "") finalString += csvSeparator;
                finalString += _string;
            }
            sw.WriteLine(finalString);
        }
    }

    public void CreateStaticFolders()
    {
        IO.CreateFolder("Assets/Editor", "Files");
        IO.CreateFolder("Assets/Editor/Files", "CSV");
    }
    public void SetHeaders()
    {
        csvHeaders = new List<string>();
        csvHeaders.Add(idName);
        csvHeaders.Add(dialogueName);
        foreach(DS_LenguageType lenguage in (DS_LenguageType[])Enum.GetValues(typeof(DS_LenguageType)))
        {
            csvHeaders.Add(lenguage.ToString());
        }
    }
}