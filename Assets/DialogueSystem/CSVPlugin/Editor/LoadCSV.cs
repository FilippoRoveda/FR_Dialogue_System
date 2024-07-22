using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


namespace CSVPlugin
{
    using DS.Editor.Enumerations;
    using DS.Editor.Data;
    using DS.Editor.ScriptableObjects;
    using DS.Editor.Utilities;
    public class LoadCSV
    {

        IOUtilities IO;
        CSVReader CSVReader;

        public LoadCSV() { CSVReader = new(); IO = new(); }

        public bool LoadAllCSVInToGraphs()
        {
            bool errorFlag = false;

            List<GraphSO> graphs = IO.LoadAssetsFromPath<GraphSO>(CSVTool.graphFilesPath);
            foreach (GraphSO graph in graphs)
            {
                LoadCSVInToGraph(graph, out errorFlag);
                IO.SaveAsset(graph);
            }
            return errorFlag;
        }
        public void LoadCSVInToGraph(GraphSO graph, out bool errorFlag)
        {
            errorFlag = false;

            var graphCSVPath = CSVTool.CSVFilesPath + graph.graphName + ".csv";
            var csvData = CSVReader.ParseCSV(File.ReadAllText(graphCSVPath));

            if (csvData == null || csvData.Count == 0)
            {
#if UNITY_EDITOR
                EditorApplication.Beep();
                EditorApplication.Beep();
                Debug.Log($"<color=red> Impossible to load CSV file at path: {graphCSVPath} </color>");
#endif
                errorFlag = true;
            }
            else
            {
                var headers = csvData[0];
                csvData.Remove(headers);

                foreach (var node in graph.GetAllOrderedNodes())
                {
                    //SKIP TO NEXT NODE IF THIS ONE HAS NOR TEXTS OR CHOICES
                    if (node.NodeType == NodeType.Branch) continue;

                    var row = csvData.Find(x => x[0] == node.NodeID);

                    if (row == null || row.Count == 0)
                    {
#if UNITY_EDITOR
                        EditorApplication.Beep();
                        EditorApplication.Beep();
                        Debug.Log($"<color=red> Impossible to load row for the node: {node.Name} with ID: {node.NodeID}. </color>");
#endif
                        errorFlag = true;
                        //continue;
                    }
                    else
                    {
                        csvData.Remove(row);
                        LoadInToNodeText(headers, row, (TextedNodeData)node);
                    }


                    //SKIP TO NEXT NODE IF THIS ONE HAS NOT CHOICES
                    if (node.NodeType == NodeType.End) continue;

                    var dialogueNode = (DialogueNodeData)node;

                    if (dialogueNode.Choices != null && dialogueNode.Choices.Count != 0)
                    {

                        foreach (var choice in dialogueNode.Choices)
                        {
                            row = csvData.Find(x => x[0] == choice.ChoiceID);

                            if (row == null || row.Count == 0)
                            {
#if UNITY_EDITOR
                                EditorApplication.Beep();
                                EditorApplication.Beep();
                                Debug.Log($"<color=red> Impossible to load row for the choice: {choice.ChoiceTexts[0]} with ID: {choice.ChoiceID}. </color>");
                                Debug.Log($"<color=red> For the node: {node.Name}. </color>");
#endif
                                errorFlag = true;
                                //continue;
                            }
                            else
                            {
                                csvData.Remove(row);
                                LoadInToChoice(headers, row, choice);
                            }
                        }
                    }
                }
            }
        }
        public void LoadInToNodeText(List<string> lenguageHeader, List<string> rowData, TextedNodeData node)
        {
            for (int i = 2; i < lenguageHeader.Count; i++)
            {
                node.Texts.Find(x => x.LenguageType.ToString() == lenguageHeader[i]).Data = rowData[i];
            }
        }
        public void LoadInToChoice(List<string> lenguageHeader, List<string> rowData, ChoiceData choice)
        {
            for (int i = 2; i < lenguageHeader.Count; i++)
            {
                choice.ChoiceTexts.Find(x => x.LenguageType.ToString() == lenguageHeader[i]).Data = rowData[i];
            }
        }
    }
}