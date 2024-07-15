using System.Collections.Generic;

namespace DS.Editor.CSV
{
    using Runtime.Data;
    using Runtime.ScriptableObjects;
    using Editor.ScriptableObjects;
    using Editor.Utilities;
    using DS.Editor.Data;
    using System.Xml;

    public class CSVLenguageHelper
    {
        IOUtilities IOUtils = new IOUtilities();
        public void UpdateLenguages()
        {

            List<DialogueContainerSO> containers = IOUtils.LoadAssetsFromPath<DialogueContainerSO>("Assets/DialogueSystem/Dialogues");
            foreach (var container in containers)
            {
                foreach (var node in container.GetAllDialogues())
                {
                    node.Texts = LenguageUtilities.UpdateLenguageDataSet(node.Texts);
                    foreach (var choice in node.Choices)
                    {
                        choice.ChoiceTexts = LenguageUtilities.UpdateLenguageDataSet(choice.ChoiceTexts);
                    }
                }
            }

            List<DS_GraphSO> graphs = IOUtils.LoadAssetsFromPath<DS_GraphSO>("Assets/Editor/Files/Graphs");
            foreach (var graph in graphs)
            {
                foreach (var node in graph.GetAllNodes())
                {
                    //SKIP TO NEXT NODE IF THIS ONE HAS NOR TEXTS OR CHOICES
                    if(node.DialogueType == Enums.DialogueType.Branch) continue;

                    var textNode = (TextedNodeData)node;
                    textNode.Texts = LenguageUtilities.UpdateLenguageDataSet(textNode.Texts);

                    //SKIP TO NEXT NODE IF THIS ONE HAS NOT CHOICES
                    if (node.DialogueType == Enums.DialogueType.End) continue;

                    var dialogueNode = (DialogueNodeData)node;

                    if (dialogueNode.Choices != null && dialogueNode.Choices.Count != 0)
                    {
                        foreach (var choice in dialogueNode.Choices)
                        {
                            choice.ChoiceTexts = LenguageUtilities.UpdateLenguageDataSet(choice.ChoiceTexts);
                        }
                    }
                }
            }
        }
    }
}