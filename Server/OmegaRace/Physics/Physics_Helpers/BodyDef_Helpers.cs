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
    class BodyDef_Helpers
    {

        public static void DynamicBoxSetup(out Body _body, PhysicBody_Data _data, GameObject gameObject,
            ref BodyDef bodyDef, float xPos_Meters, float yPos_Meters, float xSize_Meters, float ySize_Meters)
        {
            bodyDef.Position.Set(xPos_Meters, yPos_Meters);

            _body = PhysicWorld.GetWorld().CreateBody(bodyDef);
            _body.AllowSleeping(false);
            _body.SetFixedRotation(true);
            _body.SetAngle(_data.angle * PhysicWorld.MATH_PI_180);

            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(xSize_Meters / 2, ySize_Meters / 2);

            shapeDef.Density = 1.0f;
            shapeDef.Friction = 0.0f;
            shapeDef.Restitution = 0.9f;

            shapeDef.UserData = gameObject;
            shapeDef.IsSensor = _data.isSensor;

            _body.CreateFixture(shapeDef);
        }

        public static void StaticBoxSetup(out Body _body, PhysicBody_Data _data, GameObject gameObject,
            ref BodyDef bodyDef, float xPos_Meters, float yPos_Meters, float xSize_Meters, float ySize_Meters)
        {
            bodyDef.Position.Set(xPos_Meters, yPos_Meters);
            _body = PhysicWorld.GetWorld().CreateBody(bodyDef);
            _body.AllowSleeping(false);
            _body.SetAngle(_data.angle * PhysicWorld.MATH_PI_180);

            PolygonDef shapeDef = new PolygonDef();
            shapeDef.SetAsBox(xSize_Meters / 2, ySize_Meters / 2);

            //shapeDef.Density = 1.0f;
            shapeDef.Friction = 0.0f;
            shapeDef.Restitution = 0.9f;

            shapeDef.UserData = gameObject;
            shapeDef.IsSensor = _data.isSensor;

            _body.CreateFixture(shapeDef);
        }

        public static void DynamicCircleSetup(out Body _body, PhysicBody_Data _data, GameObject gameObject,
            ref BodyDef bodyDef, float xPos_Meters, float yPos_Meters)
        {
            bodyDef.Position.Set(xPos_Meters, yPos_Meters);
            _body = PhysicWorld.GetWorld().CreateBody(bodyDef);

            _body.SetAngle(_data.angle * PhysicWorld.MATH_PI_180);

            CircleDef circleDef = new CircleDef();
            circleDef.Radius = _data.radius * PhysicWorld.PIXELSTOMETERS;
            circleDef.Density = 1.0f;
            circleDef.Friction = 0.0f;
            circleDef.Restitution = 0.9f;
            circleDef.LocalPosition = new Vec2(0f, 0f);
            circleDef.UserData = gameObject;
            circleDef.IsSensor = _data.isSensor;

            _body.CreateFixture(circleDef);
        }

        public static void ShipManifoldSetup(out Body _body, PhysicBody_Data _data, GameObject gameObject,
            ref BodyDef bodyDef, float xPos_Meters, float yPos_Meters, float xSize_Meters, float ySize_Meters)
        {
            bodyDef.Position.Set(xPos_Meters, yPos_Meters);
            _body = PhysicWorld.GetWorld().CreateBody(bodyDef);
            _body.AllowSleeping(false);
            _body.SetFixedRotation(true);

            _body.SetAngle(_data.angle * PhysicWorld.MATH_PI_180);

            PolygonDef shapeDef = new PolygonDef();
            shapeDef.VertexCount = 3;
            shapeDef.Vertices[0] = new Vec2(xSize_Meters / 2, 0);
            shapeDef.Vertices[1] = new Vec2(-xSize_Meters / 2, ySize_Meters / 2);
            shapeDef.Vertices[2] = new Vec2(-xSize_Meters / 2, -ySize_Meters / 2);

            shapeDef.UserData = gameObject;
            shapeDef.IsSensor = _data.isSensor;
            shapeDef.Density = 1.0f;
            shapeDef.Friction = 0.0f;
            shapeDef.Restitution = 0.9f;


            _body.CreateFixture(shapeDef);
        }



    }
}
