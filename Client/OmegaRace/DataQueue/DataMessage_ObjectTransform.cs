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
    public class DataMessage_ObjectTransform : DataMessage
    {
        int id;

        public Vec2 pos;
        public float angle;

        //Vec2 velocity;

        public DataMessage_ObjectTransform()
            : base(MsgType.OBJECT_POS, DeliveryTarget.ClientOnly, NetDeliveryMethod.UnreliableSequenced, 3)
        {

        }

        public void Set(GameObject gObj)
        {
            this.target = DeliveryTarget.ClientOnly;

            //id = gObj.getID();
            id = gObj.getNetworkID();
            pos = gObj.GetWorldPosition();
            angle = gObj.GetAngle_Deg();
            //velocity = gObj.GetWorldVelocity();
        }


        public void Set(int _id, Vec2 _pos, float _angle/*, Vec2 _velocity*/)
        {
            this.target = DeliveryTarget.ClientOnly;

            id = _id;
            pos = _pos;
            angle = _angle;
            //velocity = _velocity;
        }

        public override void Recycle()
        {
            DataMessagePool.Return(this);
        }

        public override void Execute()
        {
            base.Execute();

            GameManager instance = GameManager.Instance();

            //GameObject gObj = instance.Find(this.id);
            GameObject gObj = instance.FindNetworkObject(this.id);

            if (gObj != null)
            {
                gObj.SetPosAndAngle(this.pos.X, this.pos.Y, this.angle);
                gObj.UpdatePosPrediction(this);
                //gObj.SetWorldVelocity(this.velocity);
            }
        }

        public override void Serialize(ref BinaryWriter writer)
        {
            base.Serialize(ref writer);
            writer.Write(this.id);
            writer.Write(this.pos.X);
            writer.Write(this.pos.Y);
            writer.Write(this.angle);
            //writer.Write(this.velocity.X);
            //writer.Write(this.velocity.Y);
        }

        public static DataMessage_ObjectTransform DeserializeDerived(ref BinaryReader reader)
        {
            int tmpId = reader.ReadInt32();
            Vec2 tmpPos = new Vec2(reader.ReadSingle(), reader.ReadSingle());
            float tmpAng = reader.ReadSingle();
            //Vec2 tmpV = new Vec2(reader.ReadSingle(), reader.ReadSingle());


            //return new DataMessage_ObjectTransform(tmpId, tmpPos, tmpAng /*, tmpV*/);
            DataMessage_ObjectTransform dm = DataMessagePool.Get_ObjectTransform();
            dm.Set(tmpId, tmpPos, tmpAng);
            return dm;
        }
    }
}
