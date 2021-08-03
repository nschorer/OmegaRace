using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    class OutputQueue
    {
        private static OutputQueue instance = null;
        public static OutputQueue Instance()
        {
            if (instance == null)
            {
                instance = new OutputQueue();
            }
            return instance;
        }

        Queue<DataMessage> pOutputQueue;

        private OutputQueue()
        {
            pOutputQueue = new Queue<DataMessage>();
        }

        public static void AddToQueue(DataMessage msg)
        {
            instance.pOutputQueue.Enqueue(msg);
        }

        public static void Process()
        {
            while (instance.pOutputQueue.Count > 0)
            {
                DataMessage msg = instance.pOutputQueue.Dequeue();

                // During playback mode, messages from output queue are ignored -- they're already in the file!
                if (DataMessage.GetMode() != DataMessage.Mode.PLAYBACK)
                {

                    bool sendToServer = ((int)msg.target & (int)DataMessage.DeliveryTarget.ServerOnly) > 0;
                    bool sendLocally = ((int)msg.target & (int)DataMessage.DeliveryTarget.ClientOnly) > 0;

                    // ...and send to server
                    if (sendToServer)
                    {
                        MyClient.Instance().SendData(msg);
                    }

                    // Process it here on server...
                    if (sendLocally)
                    {
                        InputQueue.AddToQueue(msg);
                    }

                    // Send back to corresponding object pool
                    if (!sendLocally)
                    {
                        msg.Recycle();
                    }

                }
                else
                {
                    msg.Recycle();
                }
            }


        }


    }
}
