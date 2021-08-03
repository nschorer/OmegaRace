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
    struct NetworkInfo
    {
        public string IPADDRESS;
        public int port;
    }

    class MyServer
    {
        private static MyServer instance;
        public static MyServer Instance()
        {
            if (instance == null)
            {
                instance = new MyServer();
            }
            return instance;
        }

        NetServer server;
        NetworkInfo networkInfo;

        private MyServer()
        {
            Setup();
        }

        void Setup()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connected Test");
            config.AutoFlushSendQueue = true;
            config.AcceptIncomingConnections = true;
            config.MaximumConnections = 100;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = 14240;

            server = new NetServer(config);
            server.Start();

            networkInfo = new NetworkInfo();
            networkInfo.IPADDRESS = server.Configuration.BroadcastAddress.ToString();
            networkInfo.port = 14240;

        }

        public void SendData(DataMessage msg)
        {
            if (server.ConnectionsCount > 0)
            {
                foreach (NetConnection con in server.Connections)
                {
                    NetOutgoingMessage om = server.CreateMessage();


                    MemoryStream stream = new MemoryStream();
                    BinaryWriter writer = new BinaryWriter(stream);

                    //DataMessage myMsg = new DataMessage();
                    //myMsg.horzInput = 0;
                    //myMsg.vertInput = 0;

                    //myMsg.Serialize(ref writer);

                    om.Write(stream.ToArray());

                    server.SendMessage(om, server.Connections, NetDeliveryMethod.ReliableOrdered, 4);
                }
            }
        }

        void ReadInData()
        {
            NetIncomingMessage im;
            while ((im = server.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.Data:

                        byte[] msg = im.ReadBytes(im.LengthBytes);

                        BinaryReader reader = new BinaryReader(new MemoryStream(msg));

                        DataMessage dataMsg;

                        dataMsg = DataMessage.Deserialize(ref reader);

                        break;

                }
            }
        }
    }
}

