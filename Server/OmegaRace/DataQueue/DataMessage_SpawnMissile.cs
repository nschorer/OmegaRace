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
    public class DataMessage_SpawnMissile : DataMessage
    {
        Player player;

        public DataMessage_SpawnMissile()
            : base(MsgType.SPAWN_MISSILE, DeliveryTarget.Both, NetDeliveryMethod.ReliableUnordered, 0)
        {
        }

        public void Set(Player _player)
        {
            this.target = DeliveryTarget.Both;

            player = _player;
        }

        public override void Execute()
        {
            base.Execute();

            GameManager instance = GameManager.Instance();

            switch (this.player)
            {
                case Player.ONE:
                    GameManager.FireMissile(instance.player1);
                    break;
                case Player.TWO:
                    GameManager.FireMissile(instance.player2);
                    break;
                default:
                    Debug.Assert(false, "Player not implemented");
                    break;
            }
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

        public static DataMessage_SpawnMissile DeserializeDerived(ref BinaryReader reader)
        {
            Player p = (Player)reader.ReadInt32();
            //return new DataMessage_SpawnMissile(p);
            DataMessage_SpawnMissile dm = DataMessagePool.Get_SpawnMissile();
            dm.Set(p);
            return dm;
        }
    }
}