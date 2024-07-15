using UnityEngine;
using UnityEngine.Events;

namespace DS.Runtime
{
    using Enums;
    using Runtime.Utilities;
    public class LenguageManager : Singleton<LenguageManager>
    {
        [SerializeField] private LenguageType currentLenguage = LenguageType.Italian;

        public LenguageType CurrentLenguage { get => currentLenguage; }

        public static UnityEvent<LenguageType> LenguageChanged;

        protected override void Awake()
        {
            base.Awake();
            LenguageChanged = new UnityEvent<LenguageType>();
        }
        
        public void ChangeLenguage(LenguageType newLenguage)
        {
            currentLenguage = newLenguage;
            LenguageChanged?.Invoke(currentLenguage);
        }

    }
}
