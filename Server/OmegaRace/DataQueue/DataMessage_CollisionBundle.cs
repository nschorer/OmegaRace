using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Box2DX.Common;
using Lidgren.Network;

namespace OmegaRace
{
    public class DataMessage_CollisionBundle : DataMessage
    {
        static DataMessage_CollisionBundle instance;
        List<DataMessage_Collision> list;

        private static DataMessage_CollisionBundle Instance()
        {
            if (instance == null)
            {
                instance = new DataMessage_CollisionBundle();
            }
            return instance;
        }

        private DataMessage_CollisionBundle()
            : base(MsgType.COLLISION_BUNDLE, DeliveryTarget.Both, NetDeliveryMethod.ReliableOrdered, 4)
        {
            list = new List<DataMessage_Collision>();
        }

        public override void Recycle()
        {
            Instance().target = DeliveryTarget.Both;
        }

        public static void Add(DataMessage_Collision msg)
        {
            Instance().list.Add(msg);
        }

        public static void EnqueueCollisionEvents()
        {
            if (Instance().list.Count != 0)
            {
                OutputQueue.AddToQueue(Instance());
            }
        }

        public static void Clear()
        {
            Instance().list.Clear();
        }

        public override void Execute()
        {
            base.Execute();

            foreach (DataMessage_Collision msg in list)
            {
                msg.Execute();
            }

            Clear();
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);

            writer.Write(list.Count);
            foreach (DataMessage_Collision msg in list)
            {
                msg.Serialize(ref writer);
            }
        }

        public static DataMessage_CollisionBundle DeserializeDerived(ref BinaryReader reader)
        {
            //return new DataMessage_Collision(reader.ReadInt32(), reader.ReadInt32());
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                DataMessage_Collision dm = (DataMessage_Collision)DataMessage.Deserialize(ref reader);
                Add(dm);
            }

            return Instance();
        }
    }
}