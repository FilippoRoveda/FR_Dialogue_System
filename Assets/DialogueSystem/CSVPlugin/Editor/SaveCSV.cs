using System;
using System.Collections.Generic;
using System.IO;

namespace CSVPlugin
{
    using DS.Editor.Enumerations;
    using DS.Editor.Data;
    using DS.Editor.ScriptableObjects;
    using DS.Editor.Utilities;

    public class SaveCSV
    {
        IOUtilities IO = new();

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
            List<GraphSO> graphs = IO.LoadAssetsFromPath<GraphSO>(CSVTool.graphFilesPath);

            foreach (var graph in graphs)
            {
                graphName = graph.graphName;
                CreateFile(FileName);

                foreach (var nodeData in graph.GetAllOrderedNodes())
                {
                    //SKIP TO NEXT NODE IF THIS ONE HAS NOR TEXTS OR CHOICES
                    if (nodeData.NodeType == NodeType.Branch) continue;

                    var textNode = (TextedNodeData)nodeData;
                    textNode.UpdateTextsLenguage();

                    List<string> nodeTexts = new List<string>();
                    nodeTexts.Add(nodeData.NodeID);
                    nodeTexts.Add(nodeData.Name);
                    foreach (LenguageType lenguage in (LenguageType[])Enum.GetValues(typeof(LenguageType)))
                    {
                        string lenguageText = textNode.Texts.GetLenguageData(lenguage).Data.Replace("\"", "\"\"");
                        nodeTexts.Add($"\"{lenguageText}\"");
                    }
                    AppendToFile(nodeTexts);


                    //Continue TO NEXT NODE IF THIS ONE HAS NOT CHOICES
                    if (nodeData.NodeType == NodeType.End) continue;
                    var dialogueNode = (DialogueNodeData)nodeData;

                    if (dialogueNode.Choices != null && dialogueNode.Choices.Count != 0)
                    {
                        int counter = 1;
                        foreach (var choice in dialogueNode.Choices)
                        {
                            List<string> nodeChoiceTexts = new List<string>();
                            nodeChoiceTexts.Add(choice.ChoiceID);
                            nodeChoiceTexts.Add($"_____{nodeData.Name} Choice [{counter}]");
                            counter++;
                            foreach (LenguageType lenguage in (LenguageType[])Enum.GetValues(typeof(LenguageType)))
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
            if (File.Exists(CSVTool.CSVFilesPath + fileName))
            {
                File.WriteAllText(CSVTool.CSVFilesPath + fileName, headerString);
            }
            else
            {
                using (StreamWriter sw = File.CreateText(CSVTool.CSVFilesPath + fileName))
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
            using (StreamWriter sw = File.AppendText(CSVTool.CSVFilesPath + FileName))
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
            IO.CreateFolder("Assets/Editor", CSVTool.generateDataFolderName);
            IO.CreateFolder($"Assets/Editor/{CSVTool.generateDataFolderName}", CSVTool.generatedCSVFolderName);
            IO.CreateFolder($"Assets/Editor/{CSVTool.generateDataFolderName}", CSVTool.generatedGraphsFolderName);
        }
        public void SetHeaders()
        {
            csvHeaders = new List<string>();
            csvHeaders.Add(idName);
            csvHeaders.Add(dialogueName);
            foreach (LenguageType lenguage in (LenguageType[])Enum.GetValues(typeof(LenguageType)))
            {
                csvHeaders.Add(lenguage.ToString());
            }
        }
    }
}