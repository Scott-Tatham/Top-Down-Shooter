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
        using LazyTitan.Serialization.Attributes;
        using System;
        using System.Collections;
        using System.Collections.Generic;
        using UnityEngine;
        using UnityEngine.Networking;

        #region Server

        /// <summary>
        /// Abstract server class that defines how a server behaves.
        /// </summary>
        public abstract class ServerBase : MonoBehaviour
        {
            [SerializeField, FieldProperties(RuntimeBehaviour.EDITOR)]
            protected List<ChannelInfo> channels;
            [SerializeField, FieldProperties(RuntimeBehaviour.EDITOR)]
            protected List<HostInfo> hosts;

            [SerializeField, FieldProperties("Channels", RuntimeBehaviour.RUNTIME, true)]
            protected List<ChannelID> createdChannels;
            [SerializeField, FieldProperties("Hosts", RuntimeBehaviour.RUNTIME, true)]
            protected List<HostID> createdHosts;
            [SerializeField, FieldProperties("Clients", RuntimeBehaviour.RUNTIME, true)]
            protected List<ClientInfo> clients;

            bool isStarted;

            #region ServerCreation

            void Start()
            {
                StartServer(1);
            }
            /// <summary>
            /// Create a default Connection Config.
            /// </summary>
            /// <returns></returns>
            ConnectionConfig CreateDefaultConnectionConfig()
            {
                ConnectionConfig connectionConfig = new ConnectionConfig();
                connectionConfig.ConnectTimeout = 1000;
                connectionConfig.DisconnectTimeout = 1000;
                connectionConfig.PingTimeout = 1000;
                connectionConfig.MaxConnectionAttempt = 10;

                return connectionConfig;
            }

            /// <summary>
            /// Create a custom Connection Config.
            /// </summary>
            /// <param name="connectTimeout"> Length of time a player can attempt a single connection for. </param>
            /// <param name="disconnectTimeout"> Length of time a player can attempt a single disconnection for. </param>
            /// <param name="pingTimout"> Length of time a player can attempt a single ping for. </param>
            /// <returns></returns>
            ConnectionConfig CreateConnectionConfig(uint connectTimeout = 1000, uint disconnectTimeout = 1000, uint pingTimout = 1000, byte maxConnectionAttempts = 10)
            {
                ConnectionConfig connectionConfig = new ConnectionConfig();
                connectionConfig.ConnectTimeout = connectTimeout;
                connectionConfig.DisconnectTimeout = disconnectTimeout;
                connectionConfig.PingTimeout = pingTimout;
                connectionConfig.MaxConnectionAttempt = maxConnectionAttempts;

                return connectionConfig;
            }

            #endregion

            public void StartServer(byte maxClientNumber, ConnectionConfig connectionConfig = null)
            {
                NetworkTransport.Init();

                if (connectionConfig == null)
                {
                    connectionConfig = CreateDefaultConnectionConfig();
                }

                createdChannels = Channels.CreateChannels(ref connectionConfig, channels);
                HostTopology hostTopology = new HostTopology(connectionConfig, maxClientNumber);
                createdHosts = Hosts.CreateHosts(hostTopology, hosts);
                isStarted = true;
            }

            // Add timed warnings option option and force kill.
            public IEnumerator StopServer()
            {
                // Send Disconnect

                Debug.LogError("The server is shutting down.");

                if (clients.Count > 0)
                {
                    yield return null;
                }

                Hosts.RemoveHosts(createdHosts);
                createdHosts.Clear();
                NetworkTransport.Shutdown();

                isStarted = false;
            }

            public IEnumerator StopServerWithWarnings(float warningPeriod = 60.0f, float warningIntervals = 10.0f)
            {
                Debug.LogWarning("The server is shutting down in " + warningPeriod + " seconds.");

                float intervalCount = 0;

                for (int i = 0; i > warningPeriod; i--)
                {
                    intervalCount++;

                    if (intervalCount >= warningIntervals)
                    {
                        Debug.LogWarning("The server is shutting down in " + i + " seconds.");
                        intervalCount = 0;
                    }
                }

                // Send Disconnect

                Debug.LogError("The server is shutting down.");

                if (clients.Count > 0)
                {
                    yield return null;
                }

                Hosts.RemoveHosts(createdHosts);
                createdHosts.Clear();
                NetworkTransport.Shutdown();

                isStarted = false;
            }

            public void ForceStopServer()
            {
                Debug.LogError("Networking has been force killed...");
                Hosts.RemoveHosts(createdHosts);
                createdHosts.Clear();
                NetworkTransport.Shutdown();

                isStarted = false;
            }
        }

        #endregion

        #region Client

        [Serializable]
        public struct ClientInfo
        {

        }

        /// <summary>
        /// Abstract client class that defines how a client behaves.
        /// </summary>
        public abstract class ClientBase : MonoBehaviour
        {

        }

        #endregion
    }
}