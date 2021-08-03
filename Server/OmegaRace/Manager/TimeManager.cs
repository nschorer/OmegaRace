using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OmegaRace
{
    class TimeManager
    {
        private static TimeManager instance;
        private float currentTime;
        private float t_delta;
        private int frameCount;

        public static TimeManager Instance()
        {
            if (instance == null)
            {
                instance = new TimeManager();
            }

            return instance;
        }

        private TimeManager()
        {
            t_delta = 0;
            currentTime = 0;
            frameCount = 0;
        }

        public static void Update(float _currentTime)
        {
            Instance().currentTime = _currentTime;
            Instance().frameCount++;
        }

        public static float GetCurrentTime(bool adjusted = true)
        {
            TimeManager tman = Instance();
            Debug.Assert(tman != null);

            //TimeSpan t = DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime();
            //float time = (float)t.TotalMilliseconds;

            float time = Instance().currentTime;

            if (adjusted)
            {
                time += tman.t_delta;
            }

            return time;
        }

        public static int GetFrameCount()
        {
            return Instance().frameCount;
        }

        public static void SetServerDelta(float delta)
        {
            TimeManager tman = Instance();
            Debug.Assert(tman != null);

            tman.t_delta = delta;
        }
    }
}
