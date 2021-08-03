using System;
using System.Diagnostics;
using Lidgren.Network;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;


namespace OmegaRace
{
    class NetworkGame : Azul.Game
    {
        float prevTime;

        //-----------------------------------------------------------------------------
        // Game::Initialize()
        //		Allows the engine to perform any initialization it needs to before 
        //      starting to run.  This is where it can query for any required services 
        //      and load any non-graphic related content. 
        //-----------------------------------------------------------------------------
        public override void Initialize()
        {
            // Game Window Device setup
            this.SetWindowName("Omega Race -- Server");
            this.SetWidthHeight(800, 500);
            this.SetClearColor(0.2f, 0.2f, 0.2f, 1.0f);
        }

        //-----------------------------------------------------------------------------
        // Game::LoadContent()
        //		Allows you to load all content needed for your engine,
        //	    such as objects, graphics, etc.
        //-----------------------------------------------------------------------------
        public override void LoadContent()
        {
            GameManager.Instance();

            DataMessagePool.Instance();

            PhysicWorld.Instance();
            ParticleSpawner.Instance();
            AudioManager.Instance();

            InputQueue.Instance();
            OutputQueue.Instance();

            prevTime = GetTime();
            GameManager.Start();

            TimeManager.Instance();

            DataMessage.Initialize(DataMessage.Mode.NORMAL);
            //DataMessage.Initialize(DataMessage.Mode.RECORD);
            //DataMessage.Initialize(DataMessage.Mode.PLAYBACK, "recording_.nsr");

            MyServer.Instance(); // Set up server
        }

        //-----------------------------------------------------------------------------
        // Game::Update()
        //      Called once per frame, update data, tranformations, etc
        //      Use this function to control process order
        //      Input, AI, Physics, Animation, and Graphics
        //-----------------------------------------------------------------------------

       // static int number = 0;
        public override void Update()
        {
            float curTime = GetTime();
            float gameElapsedTime = curTime - prevTime;
            prevTime = curTime;

            TimeManager.Update(curTime);

            InputManager.Update();

            PhysicWorld.Update(gameElapsedTime);
            GameManager.Update(gameElapsedTime);

            OutputQueue.Process();
            InputQueue.Process();

            MyServer.Process();

            GameManager.CleanUp();

            //Debug.WriteLine("Time: " + TimeManager.GetCurrentTime()/1000.0f);
        }

        //-----------------------------------------------------------------------------
        // Game::Draw()
        //		This function is called once per frame
        //	    Use this for draw graphics to the screen.
        //      Only do rendering here
        //-----------------------------------------------------------------------------
        public override void Draw()
        {
            GameManager.Draw();        
        }

        //-----------------------------------------------------------------------------
        // Game::UnLoadContent()
        //       unload content (resources loaded above)
        //       unload all content that was loaded before the Engine Loop started
        //-----------------------------------------------------------------------------
        public override void UnLoadContent()
        {
        }

    }
}

