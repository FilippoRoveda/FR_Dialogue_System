using Game;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace AudioSystem
{
    /// <summary>
    /// AudioManager class to manage and controll all AudioPlayer in the scene
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        /// <summary>
        /// List of channels for every possible ClipType played in the project.
        /// </summary>
        [SerializeField] public List<AudioChannel> audioChannels;
        /// <summary>
        /// Current music player of the scene.
        /// </summary>
        [SerializeField] public MusicPlayer currentMusicPlayer;


        #region Global Audio Parameters
        [Header("Global Audio Parameters")]
        [SerializeField] private float globalVolume = 100.0f;
        [SerializeField] private float musicVolume = 100.0f;
        [SerializeField] private float effectVolume = 100.0f;
        [SerializeField] private float dialogueVolume = 100.0f;
        /// <summary>
        /// Dictionary that link the type of clip with is linked volume value.
        /// </summary>
        public Dictionary<ClipType, float> audioVolumes;
        #endregion

        /// <summary>
        /// Action to inscribe an AudioPlayer in his right AudioChannel.
        /// </summary>
        public Action<AudioPlayer> subscribe;
        /// <summary>
        /// Action to remove an AudioPlayer from his right AudioChannel.
        /// </summary>
        public Action<AudioPlayer> unsubscribe;

        /// <summary>
        /// Delegate for volume changing.
        /// </summary>
        public delegate void OnVolumeChanged();
        public event OnVolumeChanged OnGlobalVolChanged;
        public event OnVolumeChanged OnMusicVolChanged;
        public event OnVolumeChanged OnEffectVolChanged;
        public event OnVolumeChanged OnDialogueVolChanged;

        #region Getter
        public float GetGlobalVolume()
        {
            return globalVolume;
        }
        public float GetMusicVolume()
        {
            return musicVolume;
        }
        public float GetEffectVolume()
        {
            return effectVolume;
        }
        public float GetDialogueVolume()
        {
            return dialogueVolume;
        }

        public MusicPlayer GetCurrentMusicPlayer()
        {
            return currentMusicPlayer;
        }
        #endregion

        /// <summary>
        /// Change volume functions that at the end call the correspondent OnVolumeChanged event.
        /// </summary>
        /// <param name="volume"></param>
        #region VolumeModifiers
        public void ChangeGlobalVolume(float volume)
        {
            globalVolume = volume;
            OnGlobalVolChanged();
        }
        public void ChangeMusicVolume(float volume)
        {
            musicVolume = volume;
            OnMusicVolChanged();
        }
        public void ChangeEffectVolume(float volume)
        {
            effectVolume = volume;
            OnEffectVolChanged();
        }
        public void ChangeDialogueVolume(float volume)
        {
            dialogueVolume = volume;
            OnDialogueVolChanged();
        }
        #endregion
        /// <summary>
        /// Set the current MusicPlayer.
        /// </summary>
        /// <param name="newPlayer"></param>
        public void SetCurrentMusicPlayer(MusicPlayer newPlayer)
        {
            currentMusicPlayer = newPlayer;
        }
        

        new private void Awake()
        {
            base.Awake();
            //Initialize audio channel for each type of clip.
            audioChannels.Add(new AudioChannel(ClipType.EFFECT));
            audioChannels.Add(new AudioChannel(ClipType.DIALOGUE));
            InitVolumes();
            //Add inscription and unsubscription function to the correspondent action.
            subscribe += Inscribe;
            unsubscribe += UnSubscribe;
        }

        /// <summary>
        /// Initialize dictionary storing volume value infomation.
        /// </summary>
        private void InitVolumes()
        {
            audioVolumes = new Dictionary<ClipType, float>();
            audioVolumes.Add(ClipType.MUSIC, GetMusicVolume());
            audioVolumes.Add(ClipType.EFFECT, GetEffectVolume());
            audioVolumes.Add(ClipType.DIALOGUE, GetDialogueVolume());
        }

        /// <summary>
        /// Inscribe an AudioPlayer to his right channel and to his OnVolumeChanged events.
        /// </summary>
        /// <param name="player"></param>
        private void Inscribe(AudioPlayer player)
        {
            foreach(AudioChannel channel in audioChannels)
            {
                if(channel.GetChannelType() == player.GetPlayerType())
                {
                    channel.AddPlayer(player);
                    return;
                }
            }
            InscribeVolume(player);
        }
        private void UnSubscribe(AudioPlayer player)
        {
            foreach (AudioChannel channel in audioChannels)
            {
                if (channel.GetChannelType() == player.GetPlayerType())
                {
                    channel.RemovePlayer(player);
                    return;
                }
            }
            UnSubscribeVolume(player);
        }

        private void UnSubscribeVolume(AudioPlayer player)
        {
            OnGlobalVolChanged -= player.ChangeVolume;
            switch (player.GetPlayerType())
            {
                case ClipType.MUSIC:
                    OnMusicVolChanged -= player.ChangeVolume;
                    break;
                case ClipType.EFFECT:
                    OnEffectVolChanged -= player.ChangeVolume;
                    break;
                case ClipType.DIALOGUE:
                    OnDialogueVolChanged -= player.ChangeVolume;
                    break;
            }
        }

        private void InscribeVolume(AudioPlayer player)
        {
            OnGlobalVolChanged += player.ChangeVolume;
            switch(player.GetPlayerType())
            {
                case ClipType.MUSIC:
                    OnMusicVolChanged += player.ChangeVolume;
                    break;
                case ClipType.EFFECT:
                    OnEffectVolChanged += player.ChangeVolume;
                    break;
                case ClipType.DIALOGUE:
                    OnDialogueVolChanged += player.ChangeVolume;
                    break;
            }
        }
    }
}
