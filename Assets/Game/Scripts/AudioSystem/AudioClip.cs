using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// Enum class to store all types of audio clip.
    /// </summary>
    public enum ClipType
    {
        EFFECT,
        DIALOGUE,
        MUSIC
    }


    /// <summary>
    /// ScriptableObject to define and store all 
    /// usefull information for an AudioClip imported in the project
    /// </summary>
    [CreateAssetMenu(menuName = "AudioClip", fileName = "new AudioClip")]
    public class AudioClip : ScriptableObject
    {
        /// <summary>
        /// Name of the clip inside the project.
        /// </summary>
        [SerializeField] private string clipName;
        /// <summary>
        /// Type of the clip inside the project.
        /// </summary>
        [SerializeField] private ClipType type;
        /// <summary>
        /// The source clip audio data
        /// </summary>
        [SerializeField] private UnityEngine.AudioClip clip;

        #region Getter
        public string GetName()
        {
            return clipName; 
        }
        public ClipType GetClipType()
        {
            return type;
        }
        public UnityEngine.AudioClip GetClip()
        {
            return clip;
        }
        #endregion
        
    }
}



