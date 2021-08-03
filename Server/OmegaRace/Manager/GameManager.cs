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
    public enum GAME_STATE
    {
        PLAY
    }

    public class GameManager 
    {
        private static GameManager instance = null;
        public static GameManager Instance()
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }

        List<GameObject> destroyList;
        List<GameObject> gameObjList;

        public Ship player1;
        public Ship player2;

        public int p2Score;
        public int p1Score;

        GameManager_UI gamManUI;

        private GameManager()
        {
            destroyList = new List<GameObject>();
            gameObjList = new List<GameObject>();

            gamManUI = new GameManager_UI();
        }

        public static void Start()
        {
            LoadLevel_Helper.LoadLevel();
        }


        public static void Update(float gameTime)
        {
            GameManager inst = Instance();
            
            inst.pUpdate();
        }

        public static void Draw()
        {
            GameManager inst = Instance();
            
            inst.pDraw();
        }

        public GameObject Find(int id)
        {
            GameObject toReturn = null;

            foreach (GameObject obj in gameObjList)
            {
                if (obj.getID() == id)
                {
                    toReturn = obj;
                    break;
                }
            }

            return toReturn;
        }
        public GameObject FindNetworkObject(int networkID)
        {
            GameObject toReturn = null;

            foreach (GameObject obj in gameObjList)
            {
                if (obj.getNetworkID() == networkID)
                {
                    toReturn = obj;
                    break;
                }
            }

            return toReturn;
        }
        public static void RecieveMessage(DataMessage msg)
        {
            if (DataMessage.GetMode() == DataMessage.Mode.RECORD)
            {
                // write to file
                DataMessage.RecordMsg(msg);
            }
            msg.Execute();
        }
        

        private void pUpdate()
        {
            if (DataMessage.GetMode() == DataMessage.Mode.PLAYBACK)
            {
                DataMessage.ExecuteNextMessage();
            }
            else
            {

                //**** Player 1 -- SERVER
                int p1_H = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P1);
                int p1_V = InputManager.GetAxis(INPUTAXIS.VERTICAL_P1);

                if (p1_H != 0 || p1_V != 0)
                {
                    //OutputQueue.AddToQueue(new DataMessage_Move(DataMessage.Player.ONE, p1_H, p1_V));
                    DataMessage_Move dm = DataMessagePool.Get_Move();
                    dm.Set(DataMessage.Player.ONE, p1_H, p1_V);
                    OutputQueue.AddToQueue(dm);
                }

                if (InputManager.GetButtonDown(INPUTBUTTON.P1_FIRE))
                {
                    //OutputQueue.AddToQueue(new DataMessage_Fire(DataMessage.Player.ONE));
                    DataMessage_Fire dm = DataMessagePool.Get_Fire();
                    dm.Set(DataMessage.Player.ONE);
                    OutputQueue.AddToQueue(dm);
                }

                // The server has authority... it tells the client what the game state is
                // Specifically, it tells the client where the ships/missiles are

                DataMessage_CollisionBundle.EnqueueCollisionEvents();

                for (int i = gameObjList.Count - 1; i >= 0; i--)
                {
                    GameObject gObj = gameObjList[i];
                    if (gObj is Ship || gObj is Missile)
                    {
                        //OutputQueue.AddToQueue(new DataMessage_ObjectTransform(gObj));
                        DataMessage_ObjectTransform dm = DataMessagePool.Get_ObjectTransform();
                        dm.Set(gObj);
                        OutputQueue.AddToQueue(dm);
                    }
                }

                //******************************

                //**** Player 2 -- CLIENT
                //int p2_H = InputManager.GetAxis(INPUTAXIS.HORIZONTAL_P2);
                //int p2_V = InputManager.GetAxis(INPUTAXIS.VERTICAL_P2);

                //if (p2_H != 0 || p2_V != 0)
                //{
                //    OutputQueue.AddToQueue(new DataMessage_Move(DataMessage.Player.TWO, p2_H, p2_V));
                //}

                //if (InputManager.GetButtonDown(INPUTBUTTON.P2_FIRE))
                //{
                //    OutputQueue.AddToQueue(new DataMessage_Fire(DataMessage.Player.TWO));
                //}

                //******************************


            }

            //**** General engine operations. No touchy!
            for (int i = gameObjList.Count - 1; i >= 0; i--)
            {
                gameObjList[i].Update();
            }
            gamManUI.Update(); // Note: Game UI (score display) is not processed as a game object
        }
        
        private void pDraw()
        {
            player1.Draw();
            player2.Draw();

            for (int i = 0; i < gameObjList.Count; i++)
            {
                gameObjList[i].Draw();
            }

            gamManUI.Draw();
        }
        


        public static void PlayerKilled(Ship s)
        {
            Instance().pPlayerKilled(s);
        }
        

        void pPlayerKilled(Ship shipKilled)
        {

            // Player 1 is Killed
            if(player1.getID() == shipKilled.getID())
            {
                p2Score++;

                player1.Respawn(new Vec2(400, 100));
                player2.Respawn(new Vec2(400, 400));
            }
            // Player 2 is Killed
            else if (player2.getID() == shipKilled.getID())
            {
                p1Score++;
                player1.Respawn(new Vec2(400, 100));
                player2.Respawn(new Vec2(400, 400));
                  
            }
        }

        public static void MissileDestroyed(Missile m)
        {
            GameManager inst = Instance();

            if (m.GetOwnerID() == inst.player1.getID())
            {
                inst.player1.GiveMissile();
            }
            else if (m.GetOwnerID() == inst.player2.getID())
            {
                inst.player2.GiveMissile();
            }
        }

        public static void FireMissile(Ship ship)
        {
            if (ship.UseMissile())
            {
                ship.Update();
                Vec2 pos = ship.GetWorldPosition();
                Vec2 direction = ship.GetHeading();
                Missile m = new Missile(new Azul.Rect(pos.X, pos.Y, 20, 5), ship.getID(), direction, ship.getColor());
                Instance().gameObjList.Add(m);
                AudioManager.PlaySoundEvent(AUDIO_EVENT.MISSILE_FIRE);
            }
        }

        

        public static void AddGameObject(GameObject obj)
        {
            Instance().gameObjList.Add(obj);
        }

        public static void CleanUp()
        {
            foreach (GameObject obj in Instance().destroyList)
            {
                Instance().gameObjList.Remove(obj);
                obj.Destroy();
            }

            Instance().destroyList.Clear();
        }
        
        public void DestroyAll()
        {
            foreach(GameObject obj in gameObjList)
            {
                destroyList.Add(obj);
            }
            gameObjList.Clear();
        }
            
        public static void DestroyObject(GameObject obj)
        {
            obj.setAlive(false);
            Instance().destroyList.Add(obj);
        }
        
        
    }
}
