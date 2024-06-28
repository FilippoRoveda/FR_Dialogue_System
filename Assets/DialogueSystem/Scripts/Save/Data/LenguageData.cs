namespace DS.Runtime.Data
{
    using Enumerations;

    [System.Serializable]
    public class LenguageData<T>
    {
        public DS_LenguageType LenguageType;
        public T Data;
    }
}
