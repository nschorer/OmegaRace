using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace OmegaRace
{
    class MyClient
    {
        private static MyClient instance;
        public static MyClient Instance()
        {
            if (instance == null)
            {
                instance = new MyClient();
            }
            return instance;
        }

        NetClient client;
        NetworkInfo myClientInfo;

        NetworkInfo connectedServerInfo;

        bool isConnected;

        private MyClient()
        {
            Setup();
        }

        public void Setup()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connected Test");
            config.AutoFlushSendQueue = true;
            config.AcceptIncomingConnections = true;
            config.MaximumConnections = 100;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = 14250;

            client = new NetClient(config);
            client.Start();

            isConnected = false;
        }
    }
}
