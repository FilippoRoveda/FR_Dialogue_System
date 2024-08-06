using UnityEngine;


namespace AudioSystem
{
    /// <summary>
    /// MusicPlayer class deriving from AudioPlayer that implement some
    /// logic in order of the different nature and role of this class inside the game.
    /// </summary>
    public class MusicPlayer : AudioPlayer
    {
        /// <summary>
        /// The type of clip played by this AudioPlayer.
        /// </summary>
        [SerializeField] private ClipType playerType = ClipType.MUSIC;

        private new void OnDisable()
        {
            //Stop the reproduction.
            Stop();
            base.OnDisable();
            //Remove this as the current AudioManger musicPlayer.
            if (AudioManager.Instance.GetCurrentMusicPlayer() == this)
            {
                AudioManager.Instance.SetCurrentMusicPlayer(null);
            }
        }
        /// <summary>
        /// Play the current stored clip after check if this MusicPlayer is the 
        /// main MusicPlayer active in AudioManager.
        /// </summary>
        public override void PlayClip()
        {
            if(AudioManager.Instance.GetCurrentMusicPlayer() != this)
            {
                SetAsMainMusicPlayer();     
            }
            base.PlayClip();
        }
        /// <summary>
        /// Play a new clip after check if this MusicPlayer is the 
        /// main MusicPlayer active in AudioManager.
        /// </summary>
        public override void PlayClip(AudioClip audioClip)
        {
            if (AudioManager.Instance.GetCurrentMusicPlayer() != this)
            {
                SetAsMainMusicPlayer();
            }
            base.PlayClip(audioClip);
        }
        public override void PlayDelayed(float delayTime = 0.0f)
        {
            if (AudioManager.Instance.GetCurrentMusicPlayer() != this)
            {
                SetAsMainMusicPlayer();
            }
            base.PlayDelayed(delayTime);
        }
        public override void PlayDelayed(AudioClip audioClip = null, float delayTime = 0)
        {
            if (AudioManager.Instance.GetCurrentMusicPlayer() != this)
            {
                SetAsMainMusicPlayer();
            }
            base.PlayDelayed(audioClip, delayTime);
        }
        public override void ReplayClip()
        {
            if (AudioManager.Instance.GetCurrentMusicPlayer() != this)
            {
                SetAsMainMusicPlayer();
            }
            base.ReplayClip();
        }

        /// <summary>
        /// Stop the previous main active MusicPlayer in AudioManager then
        /// set itself as main MusicPlayer.
        /// </summary>
        private void SetAsMainMusicPlayer()
        {
            AudioManager.Instance.GetCurrentMusicPlayer().Stop();
            AudioManager.Instance.SetCurrentMusicPlayer(this);
        }
    }
}
