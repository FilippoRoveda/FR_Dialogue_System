using System.Collections.Generic;

namespace DS.CSV
{
    using Editor.Data;
    using Editor.ScriptableObjects;
    using Editor.Utilities;


    public class CSVLenguageHelper
    {
        IOUtilities IOUtils = new IOUtilities();
        public void UpdateLenguages()
        {
            //Update only the editor graph so object, then open it and savi it overriding the generated 
            //runtime objects
            List<GraphSO> graphs = IOUtils.LoadAssetsFromPath<GraphSO>("Assets/Editor/Files/Graphs");
            foreach (var graph in graphs)
            {
                foreach (var node in graph.GetAllNodes())
                {
                    //SKIP TO NEXT NODE IF THIS ONE HAS NOR TEXTS OR CHOICES
                    if(node.DialogueType != Enums.DialogueType.Branch)
                    {
                        var textNode = (TextedNodeData)node;
                        textNode.UpdateTextsLenguage();
                    }


                    //SKIP TO NEXT NODE IF THIS ONE HAS NOT CHOICES
                    if (node.DialogueType == Enums.DialogueType.End) continue;
                    if (node.DialogueType == Enums.DialogueType.Branch) continue; // do other things

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