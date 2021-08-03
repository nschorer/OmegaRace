using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Common;
using System.Diagnostics;

namespace OmegaRace
{
    public class ObjectPositionPrediction
    {
        //public static bool predictionOn = true;

        private GameObject gObj;
        private Vec2 lastPos;
        private Vec2 lastV;
        private float lastTime;

        public ObjectPositionPrediction(Ship ship)
        {
            this.Set(ship);
        }

        public ObjectPositionPrediction(Missile missile)
        {
            this.Set(missile);
        }

        private void Set(GameObject _gObj)
        {
            gObj = _gObj;
            lastPos = gObj.GetWorldPosition();
            lastV = new Vec2(0, 0);
            lastTime = TimeManager.GetCurrentTime();
        }

        public void UpdatePrediction(DataMessage_ObjectTransform objTransform)
        {
            float now = TimeManager.GetCurrentTime();
            if (now != lastTime)
            {
                lastV = (objTransform.pos - lastPos) * (1 / (now - lastTime));

                lastPos = objTransform.pos;
                //lastTime = objTransform.time;
                lastTime = now;
            }
        }

        public void Update()
        {

                float t_delta = TimeManager.GetCurrentTime() - lastTime;
                Vec2 newPos = lastPos + t_delta * lastV;

                //Debug.WriteLine("{0}, {1}", newPos.X, newPos.Y);

                gObj.SetPosAndAngle(newPos.X, newPos.Y, gObj.GetAngle_Deg());
            
        }
    }
}
