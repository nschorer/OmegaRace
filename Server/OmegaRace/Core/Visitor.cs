using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    public abstract class Visitor
    {

        public Visitor()
        {

        }
        public virtual void Accept(GameObject obj)
        {

        }

        public virtual void VisitShip(Ship s)
        {

        }

        public virtual void VisitFence(Fence f)
        {

        }

        public virtual void VisitFencePost(FencePost fp)
        {

        }

        public virtual void VisitMissile(Missile m)
        {

        }

        

    }
}
