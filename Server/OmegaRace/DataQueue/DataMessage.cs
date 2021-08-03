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
    public class DataMessage
    {
        public enum DeliveryTarget
        {
            Delivered = 0,
            ServerOnly = 1,
            ClientOnly = 2,
            Both = 3
        }

        public enum Player
        {
            ONE,
            TWO
        }

        public enum MsgType
        {
            MOVE,
            FIRE,
            SPAWN_MISSILE,
            OBJECT_POS,
            COLLISION,
            COLLISION_BUNDLE,
            CLOCK_QUERY,
            CLOCK_RESPONSE
        }

        public enum Mode
        {
            NORMAL,
            RECORD,
            PLAYBACK
        }

        private static int msgCount = 0;
        private static Mode mode = Mode.NORMAL;
        private static BinaryWriter fwriter;
        private static BinaryReader freader;
        private static DataMessage Playback_Message;
        private static bool playbackDone = false;

        private int msgId;
        public float time;

        public MsgType type;
        public DeliveryTarget target;
        public NetDeliveryMethod deliveryMethod;
        public byte channel;

        protected DataMessage(MsgType _type, DeliveryTarget _target, NetDeliveryMethod _deliveryMethod, byte _channel)
        {
            type = _type;
            target = _target;
            deliveryMethod = _deliveryMethod;
            channel = _channel;

            msgId = ++msgCount;
        }

        public virtual void Execute()
        {
            //Debug.WriteLine("Msg Id: {0} ({1})", this.msgId, this.type);
        }

        public virtual void Recycle() { }

        public virtual void Serialize(ref BinaryWriter writer)
        {
            writer.Write((int)type);
            writer.Write(time);
            //writer.Write((int)this.target);    // no need... it's already being delivered!
        }

        public static DataMessage Deserialize(ref BinaryReader reader)
        {
            MsgType t = (MsgType)reader.ReadInt32();
            float time_ms = reader.ReadSingle();

            //DataMessage dm = new DataMessage();
            DataMessage dm = null;

            switch (t)
            {
                case MsgType.MOVE:
                    dm = DataMessage_Move.DeserializeDerived(ref reader);
                    break;
                case MsgType.FIRE:
                    dm = DataMessage_Fire.DeserializeDerived(ref reader);
                    break;
                case MsgType.SPAWN_MISSILE:
                    dm = DataMessage_SpawnMissile.DeserializeDerived(ref reader);
                    break;
                case MsgType.OBJECT_POS:
                    dm = DataMessage_ObjectTransform.DeserializeDerived(ref reader);
                    break;
                case MsgType.COLLISION:
                    dm = DataMessage_Collision.DeserializeDerived(ref reader);
                    break;
                case MsgType.COLLISION_BUNDLE:
                    dm = DataMessage_CollisionBundle.DeserializeDerived(ref reader);
                    break;
                case MsgType.CLOCK_QUERY:
                    dm = DataMessage_ClockQuery.DeserializeDerived(ref reader);
                    break;
                case MsgType.CLOCK_RESPONSE:
                    dm = DataMessage_ClockResponse.DeserializeDerived(ref reader);
                    break;
                default:
                    Debug.Assert(false, "Message type not implemented.");
                    break;
            }

            dm.target = DeliveryTarget.Delivered;
            dm.time = time_ms;

            return dm;
        }

        public static void Initialize(Mode _mode, string file = "")
        {

            DataMessage.mode = _mode;
            //DateTime dt = DateTime.Now;

            switch (DataMessage.mode)
            {
                case Mode.NORMAL:
                    break;

                case Mode.RECORD:
                    if (file == "")
                    {
                        file = "recording_" + ".nsr";
                    }

                    fwriter = new BinaryWriter(new FileStream("../bin/Debug/" + file, FileMode.Create, FileAccess.Write));

                    break;

                case Mode.PLAYBACK:
                    Debug.Assert(file != "");

                    freader = new BinaryReader(new FileStream("../bin/Debug/" + file, FileMode.Open));
                    Playback_Message = DataMessage.ReadMsgFromFile();
                    DataMessage.ExecuteNextMessage();

                    break;

                default:

                    break;
            }
        }

        public static Mode GetMode()
        {
            return DataMessage.mode;
        }

        public void SetTime()
        {
            this.time = TimeManager.GetCurrentTime();
        }

        public static void RecordMsg(DataMessage msg)
        {
            msg.time = TimeManager.GetCurrentTime();
            msg.Serialize(ref fwriter);
        }

        public static DataMessage ReadMsgFromFile()
        {
            if (freader.BaseStream.Position == freader.BaseStream.Length)
            {
                playbackDone = true;
                return null;
            }
            else
            {
                return DataMessage.Deserialize(ref freader);
            }
        }
        public static void ExecuteNextMessage()
        {
            if (!playbackDone)
            {
                float currentTime = TimeManager.GetCurrentTime();

                // Don't wait until next frame... execute all messages on current frame (in order)
                if (Playback_Message.time <= currentTime)
                {
                    Playback_Message.Execute();
                    Playback_Message = DataMessage.ReadMsgFromFile();
                    ExecuteNextMessage();
                }
            }
        }
    }
}
