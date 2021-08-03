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
        int ServerPort = 14240;

        private MyServer()
        {
            Setup();
        }

        void Setup()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connection Test");
            config.AutoFlushSendQueue = true;
            config.AcceptIncomingConnections = true;
            config.MaximumConnections = 100;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.Port = ServerPort;

            config.SimulatedLoss = 0.25f;
            config.SimulatedRandomLatency = 0.01f;

            server = new NetServer(config);
            server.Start();

            networkInfo = new NetworkInfo();
            networkInfo.IPADDRESS = server.Configuration.BroadcastAddress.ToString();
            networkInfo.port = ServerPort;

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

                    msg.Serialize(ref writer);

                    om.Write(stream.ToArray());

                    server.SendMessage(om, server.Connections, msg.deliveryMethod, msg.channel);
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
                    case NetIncomingMessageType.DiscoveryRequest:
                        Debug.WriteLine("Answering Discovery Request from " + im.SenderEndPoint);
                        NetOutgoingMessage om = server.CreateMessage();
                        om.Write("Welcome to this cool server");
                        server.SendDiscoveryResponse(om, im.SenderEndPoint);
                        break;

                    case NetIncomingMessageType.Data:

                        byte[] msg = im.ReadBytes(im.LengthBytes);

                        BinaryReader reader = new BinaryReader(new MemoryStream(msg));

                        DataMessage dataMsg;

                        dataMsg = DataMessage.Deserialize(ref reader);

                        InputQueue.AddToQueue(dataMsg);

                        break;


                    // A client's connection status changed
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        Debug.WriteLine("Connection status changed: " + status.ToString() + ": " + im.ReadString());
                        break;

                    case NetIncomingMessageType.DebugMessage:

                        break;
                }

                server.Recycle(im);
            }
        }

        public static void Process()
        {
            instance.ReadInData();
        }
    }
}

