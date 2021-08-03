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
    public class DataMessage_ClockQuery : DataMessage
    {
        float clientTime;
        public DataMessage_ClockQuery()
            : base(MsgType.CLOCK_QUERY, DeliveryTarget.ServerOnly, NetDeliveryMethod.ReliableUnordered, 0)
        {
            clientTime = TimeManager.GetCurrentTime(false);
        }

        public void Set()
        {
            this.target = DeliveryTarget.ServerOnly;
            clientTime = TimeManager.GetCurrentTime(false);
        }

        public void Set(float t0)
        {
            clientTime = t0;
        }

        public override void Execute()
        {
            base.Execute();

            DataMessage_ClockResponse dm = DataMessagePool.Get_ClockResponse();
            dm.Set(clientTime);
            OutputQueue.AddToQueue(dm);
        }

        public override void Recycle()
        {
            DataMessagePool.Return(this);
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(clientTime);
        }

        public static DataMessage_ClockQuery DeserializeDerived(ref BinaryReader reader)
        {
            //return new DataMessage_Collision(reader.ReadInt32(), reader.ReadInt32());
            DataMessage_ClockQuery dm = DataMessagePool.Get_ClockQuery();
            dm.Set(reader.ReadSingle());
            return dm;
        }
    }
}