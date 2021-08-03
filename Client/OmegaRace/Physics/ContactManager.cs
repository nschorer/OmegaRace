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

            gameObjectA.Accept(gameObjectB);
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
