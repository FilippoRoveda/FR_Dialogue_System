using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    using DS.Runtime.Enumerations;
    /// <summary>
    /// Pesisten lenguage manager that handle and execute lenguage related operations and send specific events.
    /// </summary>
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
