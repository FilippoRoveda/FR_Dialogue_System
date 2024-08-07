using System.Collections.Generic;

namespace CSVPlugin
{
    using DS.Editor.Data;
    using DS.Editor.ScriptableObjects;
    using DS.Editor.Utilities;
    using DS.Editor.Enumerations;

    public class CSVLenguageHelper
    {
        IOUtilities IOUtils = new IOUtilities();
        public void UpdateLenguages()
        {
            //Update only the editor graph so object
            List<GraphSO> graphs = IOUtils.LoadAssetsFromPath<GraphSO>("Assets/Editor/Data/Graphs");
            foreach (var graph in graphs)
            {
                foreach (var node in graph.GetAllNodes())
                {
                    //SKIP TO NEXT NODE IF THIS ONE HAS NOR TEXTS OR CHOICES
                    if(node.NodeType != NodeType.Branch)
                    {
                        var textNode = (TextedNodeData)node;
                        textNode.UpdateTextsLenguage();
                    }


                    //SKIP TO NEXT NODE IF THIS ONE HAS NOT CHOICES
                    if (node.NodeType == NodeType.End || node.NodeType == NodeType.Branch) continue;

                    var dialogueNode = (DialogueNodeData)node;

                    if (dialogueNode.Choices != null && dialogueNode.Choices.Count != 0)
                    {
                        foreach (var choice in dialogueNode.Choices)
                        {
                            choice.UpdateTextsLenguages();
                        }
                    }
                }
            }
        }
    }
}