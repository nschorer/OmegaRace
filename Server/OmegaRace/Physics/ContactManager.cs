using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;
using System.Diagnostics;

namespace OmegaRace
{
    public class ContactManager : ContactListener
    {

        public void BeginContact(Contact contact)
        {
            GameObject gameObjectA = contact._fixtureA.UserData as GameObject;
            GameObject gameObjectB = contact._fixtureB.UserData as GameObject;

            if (DataMessage.GetMode() != DataMessage.Mode.PLAYBACK)
            {
                bool skip = false;

                if (gameObjectA is Ship && gameObjectB is Missile)
                {
                    Missile m = gameObjectB as Missile;
                    Ship s = gameObjectA as Ship;
                    if (m.OwnedBy(s))
                    {
                        skip = true;
                    }
                }

                if (gameObjectB is Ship && gameObjectA is Missile)
                {
                    Missile m = gameObjectA as Missile;
                    Ship s = gameObjectB as Ship;
                    if (m.OwnedBy(s))
                    {
                        skip = true;
                    }
                }

                if (!skip)
                {
                    //gameObjectA.Accept(gameObjectB);
                    //OutputQueue.AddToQueue(new DataMessage_Collision(gameObjectA, gameObjectB));
                    DataMessage_Collision dm = DataMessagePool.Get_Collision();
                    dm.Set(gameObjectA, gameObjectB);
                    //OutputQueue.AddToQueue(dm);
                    DataMessage_CollisionBundle.Add(dm);
                }
            }
        }

        public void EndContact(Contact contact)
        {

        }

        public void PreSolve(Contact contact, Manifold manifold)
        {

        }

        public void PostSolve(Contact contact, ContactImpulse impulse)
        {

        }

    }
}