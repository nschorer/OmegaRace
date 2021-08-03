using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2DX.Dynamics;
using Box2DX.Collision;
using Box2DX.Common;

namespace OmegaRace
{
    public class PhysicWorld
    {
        // This holds, manages, and updates all the Box2d Physical bodies. 
        World privWorld;

        // Singleton Setup
        private static PhysicWorld instance;
        public static PhysicWorld Instance()
        {
            if (instance == null)
            {
                instance = new PhysicWorld();
            }
            return instance;
        }


        // Global Physics Conversion Variables
        // BIG WARNING:
        //  There are 2 types of positions for objects (World & Box2D)
        //  World: is an objects position in the world (Pixel)
        //  Box2D: is the Box2D body position in the Physics Simulation (Meters)
        //  Use these variables to convert back and worth between worlds.
        public static float PIXELSTOMETERS = 0.02f;
        public static float METERSTOPIXELS = 50.0f;
        public static float MATH_PI_180 = 0.0174532925f;
        public static float MATH_180_PI = 57.2957795147f;

        // Reference to the contact Manager.
        private ContactManager contactMan;

        // Physics World:  
        //      Job of this class is to simulate all the physical bodies that are
        //      currently in the BOX2D world through the "World" object.  All Box2D bodies that are created are placed 
        //      in the "World" object.
        private PhysicWorld()
        {
            // Define the size of the world. Simulation will still work
            // if bodies reach the end of the world, but it will be slower.
            AABB worldAABB = new AABB();
            worldAABB.LowerBound.Set(-100.0f, -100.0f);
            worldAABB.UpperBound.Set(100.0f, 100.0f);

            // Define the gravity vector.
            Vec2 gravity = new Vec2(0.0f, 0.0f);

            // Do we want to let bodies sleep?
            bool doSleep = true;

            // Construct a world object, which will hold and simulate the rigid bodies.
            privWorld = new World(worldAABB, gravity, doSleep);

            // Define the ground body.
            // BodyDef groundBodyDef = new BodyDef();
            // groundBodyDef.Position.Set(0.0f, -10.0f);

            // Call the body factory which  creates the ground box shape.
            // The body is also added to the world.
            // Body groundBody = privWorld.CreateBody(groundBodyDef);

            // Define the ground box shape.
            // PolygonDef groundShapeDef = new PolygonDef();

            // The extents are the half-widths of the box.
            // groundShapeDef.SetAsBox(50.0f, 10.0f);

            // Add the ground shape to the ground body.
            // groundBody.CreateFixture(groundShapeDef);

            contactMan = new ContactManager();

            // Starts the listener function for the contact listener
            privWorld.SetContactListener(contactMan);

        }

        // Accessor function to get the world object.
        //  Mainly used to create box2d bodies
        public static World GetWorld()
        {
            return Instance().privWorld;
        }

        public static void Update(float time)
        {
            // Prepare for simulation. Typically we use a time step of 1/60 of a
            // second (60Hz) and 10 iterations. This provides a high quality simulation
            // in most game scenarios.
            //float timeStep = 1.0f / 20.0f;
            int velocityIterations = 5;
            int positionIterations = 8;


            // Instruct the world to perform a single step of simulation. It is
            // generally best to keep the time step and iterations fixed.
            //Instance().privWorld.Step(time/60f , velocityIterations, positionIterations);
            Instance().privWorld.Step(time, velocityIterations, positionIterations);

        }

    }
}
