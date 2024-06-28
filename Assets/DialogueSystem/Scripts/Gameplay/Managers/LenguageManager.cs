using UnityEngine;

namespace DS.Runtime
{
    using Enumerations;
    using Utilities;
    public class LenguageManager : Singleton<LenguageManager>
    {
        [SerializeField] private DS_LenguageType lenguageType;

        public DS_LenguageType LenguageType { get => lenguageType; set => lenguageType = value; }


        //Method to change lenguage

        //Event to inscribe in to be aware of the lenguage change
    }
}
