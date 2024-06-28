using UnityEngine;

namespace DS.Runtime
{
    using Enumerations;
    public class LenguageController : MonoBehaviour
    {
        [SerializeField] private DS_LenguageType lenguageType;
        public static LenguageController Instance = null;

        public DS_LenguageType LenguageType { get => lenguageType; set => lenguageType = value; }

        #region Unity Callbacks
        private void Awake()
        {
            SetSingletonInstance();
        }
        #endregion

        private void SetSingletonInstance()
        {
            if (Instance == null) { Instance = this; }
            else
            {
                this.enabled = false;
                gameObject.SetActive(false);
            }
        }
    }
}
