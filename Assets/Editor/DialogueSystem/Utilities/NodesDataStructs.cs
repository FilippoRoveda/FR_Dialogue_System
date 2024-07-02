namespace DS.Editor.Utilities
{
    public static class NodesDataStructs
    {
        [System.Serializable]
        public class BaseNodeData
        { }

        [System.Serializable]
        public class StartNodeData : BaseNodeData
        { }

        [System.Serializable]
        public class SingleNodeData : BaseNodeData
        { }

        [System.Serializable]
        public class MultipleNodeData : BaseNodeData
        { }

        [System.Serializable]
        public class EventNodeData : BaseNodeData
        { }

        [System.Serializable]
        public class EndNodeData : BaseNodeData
        { }
    }
}
