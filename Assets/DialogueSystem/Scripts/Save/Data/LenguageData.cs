namespace DS.Runtime.Data
{
    using Enums;

    [System.Serializable]
    public class LenguageData<T>
    {
        public DS_LenguageType LenguageType;
        public T Data;
    }
}
