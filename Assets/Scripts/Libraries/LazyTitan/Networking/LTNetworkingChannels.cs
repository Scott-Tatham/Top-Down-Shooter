/// <summary>
/// A collection of useful code pieces.
/// </summary>
namespace LazyTitan
{
    /// <summary>
    /// Networking.
    /// </summary>
    namespace Networking
    {
        using System;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Networking;

        [Serializable]
        public struct ChannelInfo
        {
            [SerializeField]
            string channelName;
            [SerializeField]
            QosType channelQuality;

            public string GetChannelName() { return channelName; }
            public QosType GetChannelQuality() { return channelQuality; }

            public ChannelInfo(string channelName, QosType channelQuality)
            {
                this.channelName = channelName;
                this.channelQuality = channelQuality;
            }
        }

        [Serializable]
        public struct ChannelID
        {
            [SerializeField]
            string channelName;
            [SerializeField]
            int channelID;

            public string GetChannelName() { return channelName; }
            public int GetChannelID() { return channelID; }

            public ChannelID(string channelName, int channelID)
            {
                this.channelName = channelName;
                this.channelID = channelID;
            }
        }

        /// <summary>
        /// A class for creating channels to send information along.
        /// </summary>
        public static class Channels
        {
            /// <summary>
            /// Create initial channels to be used by the network.
            /// </summary>
            /// <param name="connectionConfig"> The network's ConnectionConfig. </param>
            /// <param name="channels"> The new channels to create. </param>
            /// <returns></returns>
            public static List<ChannelID> CreateChannels(ref ConnectionConfig connectionConfig, List<ChannelInfo> channels)
            {
                List<ChannelID> newChannels = new List<ChannelID>();

                for (int i = 0; i < channels.Count; i++)
                {
                    newChannels.Add(new ChannelID(channels[i].GetChannelName(), connectionConfig.AddChannel(channels[i].GetChannelQuality())));
                }

                return newChannels;
            }

            /// <summary>
            /// Add a new channel to the network.
            /// </summary>
            /// <param name="connectionConfig"> The network's ConnectionConfig. </param>
            /// <param name="channel"> The new channel to create. </param>
            /// <returns></returns>
            public static ChannelID AddChannel(ref ConnectionConfig connectionConfig, ChannelInfo channel)
            {
                return new ChannelID(channel.GetChannelName(), connectionConfig.AddChannel(channel.GetChannelQuality()));
            }

            /// <summary>
            /// Add new channels to the network.
            /// </summary>
            /// <param name="connectionConfig"> The network's ConnectionConfig. </param>
            /// <param name="channels"> The new channels to create. </param>
            /// <returns></returns>
            public static List<ChannelID> AddChannels(ref ConnectionConfig connectionConfig, List<ChannelInfo> channels)
            {
                List<ChannelID> newChannels = new List<ChannelID>();

                for (int i = 0; i < channels.Count; i++)
                {
                    newChannels.Add(new ChannelID(channels[i].GetChannelName(), connectionConfig.AddChannel(channels[i].GetChannelQuality())));
                }

                return newChannels;
            }
        }
    }
}