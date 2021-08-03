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
    public class Ship : GameObject
    {
        float maxSpeed;
        float maxForce;
        float rotateSpeed;
        Vec2 heading;

        Azul.Color shipColor;
        
        int missileCount;

        Vec2 respawnPos;
        bool respawning;

        public Ship(Azul.Rect screenRect, Azul.Color color)
            : base (GAMEOBJECT_TYPE.SHIP, new Azul.Rect(0, 0, 32, 32), screenRect, GameObject.shipTexture, color)
        {
            PhysicBody_Data data = new PhysicBody_Data();

            data.position = new Vec2(screenRect.x, screenRect.y);
            data.size = new Vec2(screenRect.width, screenRect.height);
            
            data.angle = 0;
            data.shape_type = PHYSICBODY_SHAPE_TYPE.SHIP_MANIFOLD;
            CreatePhysicBody(data);

            // maxSpeed is pixels/sec
            //maxSpeed = 150.0f;
            maxSpeed = 3.0f;

            //maxForce = 20f;
            maxForce = 0.3f;
            rotateSpeed = 5.0f;
            heading = new Vec2((float)System.Math.Cos(pBody.GetAngleDegs()), (float)System.Math.Sin(pBody.GetAngleDegs()));
            
            missileCount = 3;
            shipColor = color;

            respawnPos = new Vec2(screenRect.x, screenRect.y) ;

            networkID = NETWORKIDNUM++;
            objPosPred = new ObjectPositionPrediction(this);
        }

        public override void Update()
        {
            base.Update();
            LimitSpeed();
            UpdateDirection();

            HandleRespawn();
            objPosPred.Update();
        }


        public override void Draw()
        {
            base.Draw();
        }

        public Azul.Color getColor()
        {
            return shipColor;
        }

        public void Move(int vertInput)
        {
            if(vertInput < 0)
            {
                vertInput = 0;
            }
            pBody.ApplyForce(heading * vertInput * maxForce);
        }

        public void Rotate(int horzInput)
        {
            pBody.SetAngularVelocity(0);
            pBody.SetAngle(pBody.GetAngleDegs() + (horzInput * -rotateSpeed));
        }

        public void LimitSpeed()
        {
            Vec2 shipVel = pBody.GetBox2DVelocity();
            float magnitude = shipVel.Length();

            if(magnitude > maxSpeed)
            {
                shipVel.Normalize();
                shipVel *= maxSpeed;
                pBody.SetBox2DVelocity(shipVel);
            }
        }

        public bool UseMissile()
        {
            bool output = false;

            if (missileCount > 0)
            {
                missileCount--;
                output = true;
            }
            return output;
        }

        public int MissileCount()
        {
            return missileCount;
        }

        public void GiveMissile()
        {
            if (missileCount < 3)
            {
                missileCount++;
            }
        }
        
        public void Respawn(Vec2 v)
        {
            respawning = true;
            respawnPos = v;
        }

        private void HandleRespawn()
        {
            if(respawning == true)
            {
                pBody.SetBox2DPosition(respawnPos);
                respawning = false;
            }
        }

        void UpdateDirection()
        {
            heading = new Vec2((float)System.Math.Cos(pBody.GetAngleRads()), (float)System.Math.Sin(pBody.GetAngleRads()));
        }

        public Vec2 GetHeading()
        {
            return heading;
        }

        public void OnHit()
        {
            GameManager.PlayerKilled(this);
        }

        public override void Accept(GameObject obj)
        {
            obj.VisitShip(this);
        }

        public override void VisitMissile(Missile m)
        {
            CollisionEvent.Action(this, m);
        }
        public override void VisitFence(Fence f)
        {
            CollisionEvent.Action(f, this);
        }
    }
}
