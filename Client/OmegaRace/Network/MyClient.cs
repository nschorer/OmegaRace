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
        int ServerPort = 14240;

        private MyClient()
        {
            Setup();
        }

        public void Setup()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("Connection Test");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(config);
            client.Start();

            client.DiscoverLocalPeers(ServerPort);

            isConnected = false; // not connected until it gets discovery response
        }

        public void SendData(DataMessage msg)
        {
            if (client.ConnectionsCount > 0)
            {
                foreach (NetConnection con in client.Connections)
                {
                    NetOutgoingMessage om = client.CreateMessage();


                    MemoryStream stream = new MemoryStream();
                    BinaryWriter writer = new BinaryWriter(stream);

                    //DataMessage myMsg = new DataMessage();
                    //myMsg.horzInput = 0;
                    //myMsg.vertInput = 0;

                    msg.Serialize(ref writer);

                    om.Write(stream.ToArray());

                    client.SendMessage(om, client.Connections, msg.deliveryMethod, msg.channel);
                }
            }
        }

        void ReadInData()
        {
            NetIncomingMessage im;
            while ((im = client.ReadMessage()) != null)
            {
                switch (im.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        Debug.WriteLine("Found server at " + im.SenderEndPoint + " name: " + im.ReadString());
                        client.Connect(im.SenderEndPoint);
                        isConnected = true;

                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)im.ReadByte();
                        Debug.WriteLine("Connection status changed: " + status.ToString() + ": " + im.ReadString());

                        if (status == NetConnectionStatus.Connected)
                        {
                            // Should this be here?
                            DataMessage_ClockQuery dm = DataMessagePool.Get_ClockQuery();
                            dm.Set();
                            OutputQueue.AddToQueue(dm);
                        }

                        break;

                    case NetIncomingMessageType.Data:

                        byte[] msg = im.ReadBytes(im.LengthBytes);

                        BinaryReader reader = new BinaryReader(new MemoryStream(msg));

                        DataMessage dataMsg;

                        dataMsg = DataMessage.Deserialize(ref reader);

                        InputQueue.AddToQueue(dataMsg);

                        break;


                    case NetIncomingMessageType.DebugMessage:
                        
                        break;
                }

                client.Recycle(im);
            }
        }

        public static void Process()
        {
            instance.ReadInData();
        }

        public static bool IsConnected()
        {
            return instance.isConnected;
        }
    }
}
