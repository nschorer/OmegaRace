using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmegaRace
{
    public abstract class Particle : GameObject
    {
        public Particle(Azul.Rect textRect, Azul.Rect screenRect, Azul.Texture text, Azul.Color color)
            :base(GAMEOBJECT_TYPE.PARTICLE, textRect, screenRect, text, color)
        {

        }

    }
}
