using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// Static class with extension method to help in AudioSource management.
    /// </summary>
    public static class AudioUtils
    {
        /// <summary>
        /// Detect if this AudioSource has a pitch lower than 0.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsReversePitch(this AudioSource source)
        {
            return source.pitch < 0f;
        }

        /// <summary>
        /// Calculare the current remaining time of play of the current clip in this AudioSource.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float GetClipRemainingTime(this AudioSource source)
        {
            // Calculate the remainingTime of the given AudioSource,
            // if we keep playing with the same pitch.
            float remainingTime = (source.clip.length - source.time) / source.pitch;
            return source.IsReversePitch() ?
                (source.clip.length + remainingTime) :
                remainingTime;
        }
    }
}
