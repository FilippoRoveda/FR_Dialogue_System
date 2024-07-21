namespace DS.Runtime.Data
{
#if UNITY_STANDALONE
    using Runtime.Enumerations;

    [System.Serializable]
    public partial class LenguageData<T>
    {
        public T Data;
        public LenguageType LenguageType;   
    }
#endif
}
