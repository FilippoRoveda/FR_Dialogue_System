using UnityEngine;
using UnityEngine.Events;

namespace DS.Runtime
{
    using Enums;
    using Runtime.Utilities;
    public class LenguageManager : Singleton<LenguageManager>
    {
        [SerializeField] private DS_LenguageType currentLenguage = DS_LenguageType.Italian;

        public DS_LenguageType CurrentLenguage { get => currentLenguage; }

        public static UnityEvent<DS_LenguageType> LenguageChanged;

        protected override void Awake()
        {
            base.Awake();
            LenguageChanged = new UnityEvent<DS_LenguageType>();
        }
        
        public void ChangeLenguage(DS_LenguageType newLenguage)
        {
            currentLenguage = newLenguage;
            LenguageChanged?.Invoke(currentLenguage);
        }

    }
}
