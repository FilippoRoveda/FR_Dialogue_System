namespace DS.Editor.Windows.Utilities
{
    using Editor.Elements;
    using Editor.Data;
    using Editor.ScriptableObjects;

    public class GraphSave
    {
        private GraphSystem _system;
        public GraphSave(GraphSystem _system) { this._system = _system; }


        public void SaveGroups(GraphSO graphData)
        {
            foreach (DS_Group group in _system.groups)
            {
                SaveGroupInGraphData(group, graphData);
            }
        }
        private void SaveGroupInGraphData(DS_Group group, GraphSO graphData)
        {
            GroupData groupData = new GroupData(group.ID, group.title, group.GetPosition().position);
            graphData._groups.Add(groupData);
        }

      
        public void SaveNodes(GraphSO graphData)
        {
            foreach(var node in _system.dialogueNodes) 
            {
                DialogueNodeData nodeData = new DialogueNodeData(node);
                graphData._dialogueNodes.Add(nodeData);
            }
            foreach (var node in _system.eventNodes) 
            {
                EventNodeData eventNodeData = new EventNodeData(node);
                graphData._eventNodes.Add(eventNodeData);
            }
            foreach (var node in _system.endNodes) 
            {
                EndNodeData endNodeData = new EndNodeData(node);
                graphData._endNodes.Add(endNodeData);
            }
            foreach (var node in _system.branchNodes) 
            {
                BranchNodeData branchNodeData = new BranchNodeData(node);
                graphData._branchNodes.Add(branchNodeData);
            }

        }              
    }
}
