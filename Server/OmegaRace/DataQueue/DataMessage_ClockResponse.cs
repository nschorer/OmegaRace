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
    public class DataMessage_ClockResponse : DataMessage
    {
        float t0;
        float serverTime;
        public DataMessage_ClockResponse(float _t0)
            : base(MsgType.CLOCK_RESPONSE, DeliveryTarget.ClientOnly, NetDeliveryMethod.ReliableUnordered, 0)
        {
            t0 = _t0;
            serverTime = TimeManager.GetCurrentTime(false);
        }

        public void Set(float _t0)
        {
            this.target = DeliveryTarget.ClientOnly;

            t0 = _t0;
            serverTime = TimeManager.GetCurrentTime(false);
        }

        public void Set(float _t0, float _serverTime)
        {
            this.target = DeliveryTarget.ClientOnly;
            t0 = _t0;
            serverTime = _serverTime;
        }

        public override void Execute()
        {
            base.Execute();

            float t1 = TimeManager.GetCurrentTime(false);

            float t_client = serverTime + (t1 - t0) / 2;
            float t_delta = t_client - t1;
            TimeManager.SetServerDelta(t_delta);
        }

        public override void Recycle()
        {
            DataMessagePool.Return(this);
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(t0);
            writer.Write(serverTime);
        }

        public static DataMessage_ClockResponse DeserializeDerived(ref BinaryReader reader)
        {
            DataMessage_ClockResponse dm = DataMessagePool.Get_ClockResponse();
            dm.Set(reader.ReadSingle(), reader.ReadSingle());
            return dm;
        }
    }
}
