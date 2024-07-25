using UnityEngine;

namespace Game
{ 
    /// <summary>
    /// Singleton pattern implementation for MonoBehaviour-based classes.
    /// </summary>
    /// <typeparam name="T">Type of the singleton class.</typeparam>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// The instance of class T available from everywhere.
        /// </summary>
        public static T Instance { get; private set; } = null;

        [SerializeField] protected bool IsPersistent;

        /// <summary>
        /// Sets the single instance of the class.
        /// </summary>
        protected virtual void Awake()
        {
            SetSingleReference();
        }

        /// <summary>
        /// Sets the single instance of the class.
        /// </summary>
        protected virtual void OnEnable()
        {
            SetSingleReference();
        }

        /// <summary>
        /// Clears the instance when the application is quitting.
        /// </summary>
        private void OnApplicationQuit()
        {
            Instance = null;
        }

        /// <summary>
        /// Sets the single instance of the class and destroys redundant instances if found.
        /// </summary>
        private void SetSingleReference()
        {
            // There is already an instance
            if (Instance == this)
                return;
            else if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            // Find all instances of type T in the scenes and save them into an array
            T[] instances = FindObjectsOfType<T>();

            if (instances.Length > 1) // More than one instance is found in the scene
            {
                Debug.LogError("Multiple instances of " + typeof(T).Name + " found");
                // Set this as instance. The others will be destroyed in their Awake
                Instance = this as T;
            }
            else if (instances.Length == 1) // Only one instance of type T is found. Set the instance.
            {
                Instance = instances[0];
            }

            if (IsPersistent)
                DontDestroyOnLoad(gameObject);
        }
    }
}