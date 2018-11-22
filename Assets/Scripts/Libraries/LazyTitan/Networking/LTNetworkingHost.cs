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
        using System.Net;
        using System.Net.Sockets;
        using UnityEngine;
        using UnityEngine.Networking;

        public enum HostType
        {
            DEFAULT,
            DEFAULTSIM,
            WEB
        }

        [Serializable]
        public struct HostInfo
        {
            [SerializeField]
            string hostName;
            [SerializeField]
            string ip;
            [SerializeField]
            int port;
            [SerializeField]
            HostType hostType;
            [SerializeField]
            int minTimeout;
            [SerializeField]
            int maxTimeout;

            public string GetHostName() { return hostName; }
            public string GetIP() { return ip; }
            public int GetPort() { return port; }
            public HostType GetHostType() { return hostType; }
            public int GetMinTimeout() { return minTimeout; }
            public int GetMaxTimeout() { return maxTimeout; }

            public HostInfo(string hostName, string ip = "", int port = 0, HostType hostType = HostType.DEFAULT)
            {
                this.hostName = hostName;

                if (ip == "")
                {
                    switch (hostType)
                    {
                        case HostType.DEFAULT:
                            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                            foreach (IPAddress ipAdress in host.AddressList)
                            {
                                if (ipAdress.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    this.ip = ipAdress.ToString();
                                }
                            }

                            this.ip = "localhost";

                            break;

                        case HostType.WEB:
                            IPHostEntry webHost = Dns.GetHostEntry(Dns.GetHostName());

                            foreach (IPAddress ipAdress in webHost.AddressList)
                            {
                                if (ipAdress.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    this.ip = ipAdress.ToString();
                                }
                            }

                            this.ip = "localhost";

                            break;

                        default:
                            this.ip = "";

                            break;
                    }
                }

                else
                {
                    this.ip = ip;
                }

                this.port = port;
                this.hostType = hostType;
                minTimeout = 0;
                maxTimeout = 0;
            }

            public HostInfo(string hostName, int port = 0, HostType hostType = HostType.DEFAULT, int minTimeout = 500, int maxTimeout = 1000)
            {
                this.hostName = hostName;
                ip = "";
                this.port = port;
                this.hostType = hostType;
                this.minTimeout = minTimeout;
                this.maxTimeout = maxTimeout;
            }
        }

        [Serializable]
        public struct HostID
        {
            [SerializeField]
            string hostName;
            [SerializeField]
            int hostID;

            public string GetHostName() { return hostName; }
            public int GetHostID() { return hostID; }

            public HostID(string hostName, int hostID)
            {
                this.hostName = hostName;
                this.hostID = hostID;
            }
        }

        /// <summary>
        /// A class for creating channels to send information along.
        /// </summary>
        public static class Hosts
        {
            /// <summary>
            /// Create initial hosts to be used by the network.
            /// </summary>
            /// <param name="hostTopology"> The network's topology. </param>
            /// <param name="hosts"> The new hosts to create. </param>
            /// <returns></returns>
            public static List<HostID> CreateHosts(HostTopology hostTopology, List<HostInfo> hosts)
            {
                List<HostID> newHosts = new List<HostID>();

                for (int i = 0; i < hosts.Count; i++)
                {
                    HostID newHost = SetHost(hostTopology, hosts[i]);
                    newHosts.Add(newHost);
                }

                return newHosts;
            }

            /// <summary>
            /// Add a new host to the network.
            /// </summary>
            /// <param name="hostTopology"> The network's topology. </param>
            /// <param name="host"> The new host to create. </param>
            /// <returns></returns>
            public static HostID AddHost(HostTopology hostTopology, HostInfo host)
            {
                return SetHost(hostTopology, host);
            }

            /// <summary>
            /// Add new hosts to the network.
            /// </summary>
            /// <param name="hostTopology"> The network's topology. </param>
            /// <param name="hosts"> The new hosts to create. </param>
            /// <returns></returns>
            public static List<HostID> AddHosts(HostTopology hostTopology, List<HostInfo> hosts)
            {
                List<HostID> newHosts = new List<HostID>();

                for (int i = 0; i < hosts.Count; i++)
                {
                    HostID newHost = SetHost(hostTopology, hosts[i]);
                    newHosts.Add(newHost);
                }

                return newHosts;
            }

            /// <summary>
            /// Removes hosts from the network. This doesn't remove their entry from the supplied list.
            /// </summary>
            /// <param name="hosts"> The hosts to remove. </param>
            public static void RemoveHosts(List<HostID> hosts)
            {
                for (int i = 0; i < hosts.Count; i++)
                {
                    Debug.Log("Removing Host: " + hosts[i].GetHostName() + " from the network...");
                    NetworkTransport.RemoveHost(hosts[i].GetHostID());
                }
            }

            /// <summary>
            /// Filter HostType to determine the type of host.
            /// </summary>
            /// <param name="hostTopology"> The network's topology. </param>
            /// <param name="host"> The new host to create. </param>
            /// <returns></returns>
            static HostID SetHost(HostTopology hostTopology, HostInfo host)
            {
                switch (host.GetHostType())
                {
                    case HostType.DEFAULT:
                        return new HostID(host.GetHostName(), NetworkTransport.AddHost(hostTopology, host.GetPort(), host.GetIP()));

                    case HostType.DEFAULTSIM:
                        return new HostID(host.GetHostName(), NetworkTransport.AddHostWithSimulator(hostTopology, host.GetMinTimeout(), host.GetMaxTimeout(), host.GetPort()));

                    case HostType.WEB:
                        return new HostID(host.GetHostName(), NetworkTransport.AddWebsocketHost(hostTopology, host.GetPort(), host.GetIP()));

                    default:
                        Debug.LogWarning("The HostType was unexpected, setting to default host.");

                        return new HostID(host.GetHostName(), NetworkTransport.AddHost(hostTopology, host.GetPort(), host.GetIP()));
                }
            }
        }
    }
}