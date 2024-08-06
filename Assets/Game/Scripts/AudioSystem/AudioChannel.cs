using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    /// <summary>
    /// AudioChannel class to contain all AudioPlayer playing the same ClipType, can manage them all.
    /// </summary>
    [System.Serializable]
    public class AudioChannel
    {
        /// <summary>
        /// The type of ClipType player by AudioPlayer in the list.
        /// </summary>
        [SerializeField] private ClipType channelType;
        /// <summary>
        /// All AudioPlayer of a certain ClipType in the current scene.
        /// </summary>
        [SerializeField] private List<AudioPlayer> channelPlayers;
        #region Getter
        public ClipType GetChannelType()
        {
            return channelType;
        }
        #endregion

        public AudioChannel(ClipType type, List<AudioPlayer> players = null)
        {//If the type result as MUSIC convert it at EFFECT to prevent error, MusicPlayer is the right class for this
         //type of audio and is managed in different ways.
            if (type != ClipType.MUSIC)
            {
                channelType = type;
            }
            else { channelType = ClipType.EFFECT;}
            if(players == null)
            {
                channelPlayers = new List<AudioPlayer>();
            }
            else
            {
                channelPlayers = players;
            }
        }

        /// <summary>
        /// Add new player to the list if not already contained.
        /// </summary>
        /// <param name="newPlayer"></param>
        public void AddPlayer(AudioPlayer newPlayer)
        {
            if (!channelPlayers.Contains(newPlayer))
            {
                channelPlayers.Add(newPlayer);
            }
            else
            {
                Debug.LogError("AudioPlayer is already in channelPlayers list.");
            }
        }

        public void AddPlayers(List<AudioPlayer> newPlayers)
        {
            foreach(AudioPlayer player in newPlayers)
            {
                AddPlayer(player);
            }
        }
        /// <summary>
        /// Remove a player from the list if contained.
        /// </summary>
        /// <param name="player"></param>
        public void RemovePlayer(AudioPlayer player)
        {
            if (!channelPlayers.Contains(player))
            {
                channelPlayers.Remove(player);
            }
            else
            {
                Debug.LogError("No correspondent player founded in channel.");
            }
        }

        /// <summary>
        /// Clear the player list.
        /// </summary>
        public void RemoveAll()
        {
            channelPlayers.Clear();
        }
        public void PlayAll()
        {
            foreach(AudioPlayer player in channelPlayers)
            {
                player.PlayClip();
            }
        }
        public void RePlayAll()
        {
            foreach (AudioPlayer player in channelPlayers)
            {
                player.ReplayClip();
            }
        }
        public void StopAll(float delayTime = 0.0f)
        {
            foreach (AudioPlayer player in channelPlayers)
            {
                player.Stop(delayTime);
            }
        }
        public void PauseAll()
        {
            foreach (AudioPlayer player in channelPlayers)
            {
                player.Pause();
            }
        }
        public void UnPauseAll()
        {
            foreach (AudioPlayer player in channelPlayers)
            {
                player.UnPause();
            }
        }
    }
}
