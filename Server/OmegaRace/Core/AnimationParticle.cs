using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace OmegaRace
{
    public class AnimationParticle : Particle
    {
        Stopwatch timer;
        List<Azul.Sprite> animation;

        int frame;
        float frameSpeed;

        public AnimationParticle(Azul.Rect textRect, Azul.Rect destRect, Azul.Texture text, Azul.Color color)
            : base(textRect, destRect, text, color)
        {
            timer = new Stopwatch();
            frame = 0;
            timer.Start();
            frameSpeed = 1.0f * 100;
            animation = new List<Azul.Sprite>();
            animation.Add(pSprite);
            //Debug.WriteLine("New particle: " + this.getID() + " (frame {0})", TimeManager.GetFrameCount());
        }

        public void setAnimation(List<Azul.Sprite> anim)
        {
            animation = anim;
        }

        public override void Update()
        {
            TimeSpan sp = timer.Elapsed;

            if(frameSpeed < sp.Milliseconds)
            {
                timer.Restart();
                frame++;
                if (frame < animation.Count)
                {
                    pSprite = animation[frame];
                }
                else
                {
                    GameManager.DestroyObject(this);
                }
            }

            base.Update();
        }

        public override void Draw()
        {
            pSprite.Render();

        }

    }
}
