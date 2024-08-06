using System.Collections;
using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// AudioPlayer class that encapsulate and manage an AudioSource component,
    /// is linked to play only one type of AudioClip during all his lifecicle.
    /// </summary>
    public class AudioPlayer : MonoBehaviour
    {
        /// <summary>
        /// The type of clip played by this AudioPlayer.
        /// </summary>
        [SerializeField] private ClipType playerType;
        /// <summary>
        /// AudioSource linked to this AudioPlayer.
        /// </summary>
        [SerializeField] protected AudioSource source = null;
        /// <summary>
        /// CurrentClip stored in the class for future use.
        /// </summary>
        [SerializeField] protected AudioClip currentClip = null;


        public delegate void AudioEvent();
        /// <summary>
        /// Event that detect the end of the current clip played.
        /// </summary>
        public event AudioEvent OnClipEnded;

        /// <summary>
        /// Cache WaitForSeconds to avoid garbage collecting.
        /// </summary>
        private WaitForSeconds waitForSeconds;
        private float lastClipLenght = 0.0f;

        #region Getter
        public ClipType GetPlayerType()
        {
            return playerType;
        }
        #endregion

        protected void Awake()
        {
            //If there are not already an AudioSource on the object create it
            if (GetComponent<AudioSource>() != null)
            {
                source = GetComponent<AudioSource>();
            }
            else
            {
                source = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
            }
        }
        protected void OnEnable()
        {
            AudioManager.Instance.subscribe?.Invoke(this);
        }

        protected void OnDisable()
        {
            AudioManager.Instance.unsubscribe?.Invoke(this);
        }

        /// <summary>
        /// Get the proper volume value by the AudioManger in scene.
        /// </summary>
        public void ChangeVolume()
        {
          source.volume =  (AudioManager.Instance.GetGlobalVolume() *
                            AudioManager.Instance.audioVolumes[playerType]) 
                            / 100.0f;
        }
        /// <summary>
        /// Overload to change volume using a desired value.
        /// </summary>
        /// <param name="newVolume"></param>
        public void ChangeVolume(float newVolume = 0.0f)
        {
            source.volume = newVolume;
        }
        /// <summary>
        /// Set the current clip stored in the class.
        /// </summary>
        /// <param name="nextClip"></param>
        /// <returns>Return True if operation succede.</returns>
        public bool ChangeClip(AudioClip nextClip)
        {
            //Check if the new clip type is the same as the Player type.
            if (nextClip.GetClipType() == playerType)
            {
                currentClip = nextClip;
                source.clip = nextClip.GetClip();
                return true;
            }
            else
            {
                Debug.LogError("ClipType invalid for this AudioPlayer.");
                return false;
            }
        }

        /// <summary>
        /// Play the current stored clip if it's not null.
        /// </summary>
        public virtual void PlayClip()
        {
            if (source.isPlaying == false && currentClip != null)
            {
                source.Play();
                StartCoroutine(ClipTimer());
            }
            else
            {
                Debug.LogError("No clip to play in AudioPlayer or player is still playing a clip.");
            }
        }
        /// <summary>
        /// Play passed clip.
        /// </summary>
        /// <param name="audioClip"></param>
        public virtual void PlayClip(AudioClip audioClip)
        {
            if (source.clip != audioClip)
            {
                if(ChangeClip(audioClip) == true)
                {
                    PlayClip();
                }
            }
            else
            {
                Debug.LogError("Passed the same clip already in use in this AudioPlayer.");
                PlayClip();
            }
        }

        /// <summary>
        /// Play the current stored clip with a delay.
        /// </summary>
        /// <param name="delayTime"></param>
        public virtual void PlayDelayed(float delayTime = 0.0f)
        {
            if (source.isPlaying == false && currentClip != null)
            {
                source.PlayDelayed(delayTime);
                StartCoroutine(ClipTimer());
            }
            else
            {
                Debug.LogError("No clip to play in AudioPlayer or player is still playing a clip.");
            }
        }
        /// <summary>
        /// Play a new passed clip.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="delayTime"></param>
        public virtual void PlayDelayed(AudioClip audioClip = null, float delayTime = 0.0f)
        {
            if (source.isPlaying == false && source.clip != audioClip)
            {
                if (ChangeClip(audioClip) == true)
                {
                    PlayDelayed(delayTime);
                }
            }
            else
            {
                Debug.LogError("Passed the same clip already in use in this AudioPlayer or player is still playing a clip..");
            }
        }

        public virtual void ReplayClip()
        {
            if (currentClip != null)
            {
                Stop();
                PlayClip();
            }
            else
            {
                Debug.LogError("No clip to re-play in AudioPlayer.");
            }
        }

        public void Pause()
        {
            if (source.isPlaying == true && currentClip != null)
            {
                source.Pause();
                
            }
            else
            {
                Debug.LogError("No clip to pause in AudioPlayer or no clip is playing in this moment.");
            }
        }
        
        public void UnPause()
        {
            if (source.isPlaying == false && currentClip != null)
            {
                source.UnPause();
            }
            else
            {
                Debug.LogError("No clip to un-pause in AudioPlayer or clip is already playing in this moment.");
            }
        }

        public void Mute()
        {
            source.mute = true;
        }
        public void UnMute()
        {
            source.mute = false;
        }
        public void Stop(float delayTime = 0.0f)
        {
            if (delayTime == 0.0f)
            {
                source.Stop();
                StopCoroutine(ClipTimer());
            }
            else
            {
                StartCoroutine(DelayedStop(delayTime));
            }
        }

        /// <summary>
        /// Delay the stop of the current reproduction.
        /// </summary>
        /// <param name="delayTime"></param>
        /// <returns></returns>
        protected IEnumerator DelayedStop(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            Stop();
        }

        /// <summary>
        /// Coroutine that at his end call OnClipEnded event
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ClipTimer()
        {
            if (source.GetClipRemainingTime() != lastClipLenght)
            {
                waitForSeconds = new WaitForSeconds(source.GetClipRemainingTime());
                lastClipLenght = source.GetClipRemainingTime();
            }
            yield return waitForSeconds;
            OnClipEnded();
        }

        //Fade method?
    }

}