using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    class MsgPool
    {


        private static MsgPool instance;
        public static MsgPool Instance()
        {
            if (instance == null)
            {
                instance = new MsgPool();
            }
            return instance;
        }

        private List<DataMessage> msgList;

        private MsgPool()
        {
            msgList = new List<DataMessage>();
            DataMessage[] arr = new DataMessage[20];

            msgList.AddRange(arr);
        }

    }
}
