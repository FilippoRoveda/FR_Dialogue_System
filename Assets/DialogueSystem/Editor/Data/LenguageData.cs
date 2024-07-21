namespace DS.Editor.Data
{
#if UNITY_EDITOR
    using Editor.Enumerations;

    [System.Serializable]
    public partial class LenguageData<T>
    {
        public T Data;
        public LenguageType LenguageType;   
    }
#endif
}
