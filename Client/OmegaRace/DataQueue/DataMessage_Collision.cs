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
    public class DataMessage_Collision : DataMessage
    {
        int id1;
        int id2;

        public DataMessage_Collision()
            : base(MsgType.COLLISION, DeliveryTarget.Both, NetDeliveryMethod.ReliableOrdered, 4)
        {
        }

        public void Set(GameObject gObj1, GameObject gObj2)
        {
            this.target = DeliveryTarget.Both;
            //id1 = gObj1.getID();
            //id2 = gObj2.getID();
            id1 = gObj1.getNetworkID();
            id2 = gObj2.getNetworkID();
        }

        public void Set(int _id1, int _id2)
        {
            this.target = DeliveryTarget.Both;
            id1 = _id1;
            id2 = _id2;
        }

        public override void Execute()
        {
            base.Execute();

            GameManager instance = GameManager.Instance();

            //GameObject gObj1 = instance.Find(this.id1);
            //GameObject gObj2 = instance.Find(this.id2);
            GameObject gObj1 = instance.FindNetworkObject(this.id1);
            GameObject gObj2 = instance.FindNetworkObject(this.id2);

            //Debug.WriteLine("Collision: {0}, {1} -- frame {2}", this.id1, this.id2, TimeManager.GetFrameCount());

            if (gObj1 != null && gObj2 != null)
            {
                gObj1.Accept(gObj2);
            }
            else
            {
                //Debug.WriteLine("Collision Failed: {0}, {1} -- frame {2}", this.id1, this.id2, TimeManager.GetFrameCount());
                this.target = DeliveryTarget.ClientOnly;
                OutputQueue.AddToQueue(this);
            }
        }

        public override void Recycle()
        {
            DataMessagePool.Return(this);
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(this.id1);
            writer.Write(this.id2);
        }

        public static DataMessage_Collision DeserializeDerived(ref BinaryReader reader)
        {
            //return new DataMessage_Collision(reader.ReadInt32(), reader.ReadInt32());
            DataMessage_Collision dm = DataMessagePool.Get_Collision();
            dm.Set(reader.ReadInt32(), reader.ReadInt32());
            return dm;
        }
    }
}