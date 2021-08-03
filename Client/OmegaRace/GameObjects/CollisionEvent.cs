using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    static class CollisionEvent
    {
        public static void Action(Fence f, Missile m)
        {
            m.OnHit();
            f.OnHit();
        }

        public static void Action(Fence f, Ship s)
        {
            f.OnHit();
        }

        public static void Action(FencePost f, Missile m)
        {
            m.OnHit();
        }

        public static void Action(Ship s, Missile m)
        {
            if (!m.OwnedBy(s))
            {
                m.OnHit();
                s.OnHit();
            }
        }
    }
}
