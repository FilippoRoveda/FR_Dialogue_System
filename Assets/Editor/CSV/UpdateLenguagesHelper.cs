using DS.Runtime.ScriptableObjects;
using System.Collections.Generic;
using DS.Runtime.Data;
using DS.Editor.ScriptableObjects;
using DS.Editor.Windows.Utilities;

public class UpdateLenguagesHelper
{
    DS_IOUtilities IOUtils = new DS_IOUtilities();
    public void UpdateLenguages()
    {
        //List<DS_DialogueContainerSO> containers = IOUtils.LoadAssetsByType<DS_DialogueContainerSO>();
        List<DS_DialogueContainerSO> containers = IOUtils.LoadAssetsFromPath<DS_DialogueContainerSO>("Assets/DialogueSystem/Dialogues");
        foreach (var container in containers)
        {
            foreach(var node in container.GetAllDialogues())
            {
                node.Texts = DS_LenguageUtilities.UpdateLenguageDataSet(node.Texts);
                foreach (var choice in node.Choices)
                {
                    choice.ChoiceTexts = DS_LenguageUtilities.UpdateLenguageDataSet(choice.ChoiceTexts);
                }
            }
        }
        //List<DS_GraphSO> graphs = IOUtils.LoadAssetsByType<DS_GraphSO>();
        List<DS_GraphSO> graphs = IOUtils.LoadAssetsFromPath<DS_GraphSO>("Assets/Editor/Files/Graphs");
        foreach (var graph in graphs)
        {
            foreach (var node in graph.GetAllNodes()) 
            {
                node.Texts = DS_LenguageUtilities.UpdateLenguageDataSet(node.Texts);
                if(node.Choices != null && node.Choices.Count != 0)
                {
                    foreach (var choice in node.Choices)
                    {
                        choice.ChoiceTexts = DS_LenguageUtilities.UpdateLenguageDataSet(choice.ChoiceTexts);
                    }
                }
            }
        }
    }
}
