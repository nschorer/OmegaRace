using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;

namespace OmegaRace
{
    public enum PARTICLE_EVENT
    {
        EXPLOSION,
        FENCE_HIT
    }


    public class ParticleSpawner 
    {
        private static ParticleSpawner instance = null;
        public static ParticleSpawner Instance()
        {
            if (instance == null)
            {
                instance = new ParticleSpawner();
            }
            return instance;
        }

        private static Azul.Texture explosionText = new Azul.Texture("explosion.tga");
        private static Azul.Texture fenceText1 = new Azul.Texture("FenceTall1.tga");
        private static Azul.Texture fenceText2 = new Azul.Texture("FenceTall2.tga");
        private static Azul.Texture fenceText3 = new Azul.Texture("FenceTall3.tga");
        private static Azul.Texture fenceText4 = new Azul.Texture("FenceTall4.tga");
        private static Azul.Texture fenceText5 = new Azul.Texture("FenceTall5.tga");
        private static Azul.Texture fenceText6 = new Azul.Texture("FenceTall6.tga");
        private static Azul.Texture fenceText7 = new Azul.Texture("FenceTall7.tga");

        private ParticleSpawner()
        {

        }


        public static void SpawnParticleEvent(PARTICLE_EVENT Event_Type, GameObject Sender)
        {
            ParticleSpawner inst = Instance();
            switch(Event_Type)
            {
                case PARTICLE_EVENT.EXPLOSION:
                    inst.SpawnExplosionParticle(Sender);
                    break;
                case PARTICLE_EVENT.FENCE_HIT:
                    inst.SpawnFenceParticle(Sender as Fence);
                    break;
            }
        }

        void SpawnExplosionParticle(GameObject obj)
        {
            Vec2 pos = obj.GetWorldPosition();
            Azul.Rect dest = new Azul.Rect(pos.X, pos.Y, 30, 30);
            AnimationParticle part = new AnimationParticle(new Azul.Rect(0, 0, 86, 70), dest, explosionText, new Azul.Color(1,1,1));
            GameManager.AddGameObject(part);
        }

        void SpawnFenceParticle(Fence obj)
        {
            Vec2 pos = obj.GetWorldPosition();
            Azul.Rect source = new Azul.Rect(0, 0, 6, 209);
            Azul.Color color = new Azul.Color(1, 1, 1);
            Azul.Rect dest = new Azul.Rect(pos.X, pos.Y, 1, 1);

            Azul.Rect objDimensions = obj.getWorldRect();
            objDimensions = new Azul.Rect(objDimensions.x, objDimensions.y, objDimensions.width, objDimensions.height);


            Azul.Sprite s1 = new Azul.Sprite(fenceText1, source, objDimensions, color);
            Azul.Sprite s2 = new Azul.Sprite(fenceText2, source, objDimensions, color);
            Azul.Sprite s3 = new Azul.Sprite(fenceText3, source, objDimensions, color);
            Azul.Sprite s4 = new Azul.Sprite(fenceText4, source, objDimensions, color);
            Azul.Sprite s5 = new Azul.Sprite(fenceText5, source, objDimensions, color);
            Azul.Sprite s6 = new Azul.Sprite(fenceText6, source, objDimensions, color);
            Azul.Sprite s7 = new Azul.Sprite(fenceText7, source, objDimensions, color);

            s1.angle = obj.GetAngle_Rad();
            s2.angle = obj.GetAngle_Rad();
            s3.angle = obj.GetAngle_Rad();
            s4.angle = obj.GetAngle_Rad();
            s5.angle = obj.GetAngle_Rad();
            s6.angle = obj.GetAngle_Rad();
            s7.angle = obj.GetAngle_Rad();

            List<Azul.Sprite> anim = new List<Azul.Sprite>();
            anim.Add(s1);
            anim.Add(s2);
            anim.Add(s3);
            anim.Add(s4);
            anim.Add(s5);
            anim.Add(s6);
            anim.Add(s7);

            AnimationParticle part = new AnimationParticle(source, dest, fenceText1, color);
            part.setAnimation(anim);

            GameManager.AddGameObject(part);
        }



    }
}
