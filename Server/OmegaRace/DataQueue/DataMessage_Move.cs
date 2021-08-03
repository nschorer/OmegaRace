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
    public class DataMessage_Move : DataMessage
    {
        Player player;
        public int hIn;
        public int vIn;

        public DataMessage_Move()
            : base(MsgType.MOVE, DeliveryTarget.Both, NetDeliveryMethod.ReliableOrdered, 4)
        {
        }

        public void Set(Player _player, int h, int v)
        {
            this.target = DeliveryTarget.Both;
            player = _player;
            hIn = h;
            vIn = v;
        }

        public override void Execute()
        {
            base.Execute();

            GameManager instance = GameManager.Instance();

            switch (player)
            {
                case Player.ONE:
                    instance.player1.Rotate(hIn);
                    instance.player1.Move(vIn);
                    break;
                case Player.TWO:
                    instance.player2.Rotate(hIn);
                    instance.player2.Move(vIn);
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
            writer.Write((int)player);
            writer.Write(hIn);
            writer.Write(vIn);
        }

        public static DataMessage_Move DeserializeDerived(ref BinaryReader reader)
        {
            Player p = (Player)reader.ReadInt32();
            int h = reader.ReadInt32();
            int v = reader.ReadInt32();

            //return new DataMessage_Move(p, h, v);
            DataMessage_Move dm = DataMessagePool.Get_Move();
            dm.Set(p, h, v);
            return dm;
        }
    }
}