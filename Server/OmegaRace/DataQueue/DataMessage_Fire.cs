using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Lidgren.Network;

namespace OmegaRace
{
    public class DataMessage_Fire : DataMessage
    {
        Player player;

        public DataMessage_Fire()
            : base(MsgType.FIRE, DeliveryTarget.ServerOnly, NetDeliveryMethod.ReliableUnordered, 0)
        {
        }

        public void Set(Player _player)
        {
            this.target = DeliveryTarget.ServerOnly;
            player = _player;
        }

        public override void Execute()
        {
            base.Execute();

            //OutputQueue.AddToQueue(new DataMessage_SpawnMissile(this.player));
            DataMessage_SpawnMissile dm = DataMessagePool.Get_SpawnMissile();
            dm.Set(this.player);
            OutputQueue.AddToQueue(dm);
        }

        public override void Recycle()
        {
            DataMessagePool.Return(this);
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);

            writer.Write((int)this.player);
        }

        public static DataMessage_Fire DeserializeDerived(ref BinaryReader reader)
        {
            //return new DataMessage_Fire((Player)reader.ReadInt32());
            DataMessage_Fire dm = DataMessagePool.Get_Fire();
            dm.Set((Player)reader.ReadInt32());
            return dm;
        }
    }
}