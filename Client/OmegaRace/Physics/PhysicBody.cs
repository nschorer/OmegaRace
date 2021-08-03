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

    // Enum to represent the shape of the object
    public enum PHYSICBODY_SHAPE_TYPE
    {
        NULL,
        DYNAMIC_BOX,
        DYNAMIC_CIRCLE,
        STATIC_BOX,
        STATIC_CIRCLE,
        SHIP_MANIFOLD
    };
    
    // All Box2D Bodies have a lot settings that must be set in order for it to be created
    //  This struct is used to set those settings (See BodyDef_Helpers.cs) I use them there.
    //  SIDE NOTE: There are 2 types of positions (World & Box2D)
    //      World: is the game world position (Pixels)
    //      Box2D: is the box2d position    (Meters)
    public struct PhysicBody_Data
    {
        // Set world position (PIXELS) of the object (Converts to Meters when passed to Physics_Body Constructor)
        public Vec2 position;
        // Set world size (PIXELS) of the object (Converts to Meters when passed to Physics_Body Constructor)
        public Vec2 size;
        // Set world radius (if its a circle) (PIXELS) of the object (Converts to Meters when passed to Physics_Body Constructor)
        public float radius;
        // Sets liner damping of the object.  "Think wind resistance"
        public float linearDamping;
        // Sets angular damping of the object
        public float angularDamping;
        // Sets starting angle (Degrees).  (Converts to Radians when passed to Physics_Body Constructor)
        public float angle;
        // Defines the shape of the object
        public PHYSICBODY_SHAPE_TYPE shape_type;
        // Whether or not this object is a "Trigger" instead of collision.
        //  Collision: Collision behaves as normal and triggers the contact manager
        //  Trigger:   Triggers the contact manager but does not collide with objects on a physical level
        public bool isSensor;
    };


    public class PhysicBody
    {
        // Refernce to the BOX2D body 
        Body pBody;

        public PhysicBody(PhysicBody_Data data, GameObject gameObject)
        {
            // Converting World Position from Pixels to Meters
            float xMeters = PhysicWorld.PIXELSTOMETERS * data.position.X;
            float yMeters = PhysicWorld.PIXELSTOMETERS * data.position.Y;

            // Converting World Size from Pixels to Meters 
            float sizeXMeters = PhysicWorld.PIXELSTOMETERS * data.size.X;
            float sizeYMeters = PhysicWorld.PIXELSTOMETERS * data.size.Y;

            // Defines the Physical Body Definition  
            //  The "Shape" of the object. See "PhysicBody Data"
            BodyDef bodyDef = new BodyDef();
            
            Debug.Assert(data.shape_type != PHYSICBODY_SHAPE_TYPE.NULL);

            // Setup for BodyDef depending on shape
            //
            if (data.shape_type == PHYSICBODY_SHAPE_TYPE.DYNAMIC_BOX)
            {
                BodyDef_Helpers.DynamicBoxSetup(out pBody, data, gameObject, ref bodyDef, xMeters, yMeters, sizeXMeters, sizeYMeters);
            }
            else if(data.shape_type == PHYSICBODY_SHAPE_TYPE.DYNAMIC_CIRCLE)
            {
                BodyDef_Helpers.DynamicCircleSetup(out pBody, data, gameObject, ref bodyDef, xMeters, yMeters);
            }
            else if(data.shape_type == PHYSICBODY_SHAPE_TYPE.STATIC_BOX)
            {
                BodyDef_Helpers.StaticBoxSetup(out pBody, data, gameObject, ref bodyDef, xMeters, yMeters, sizeXMeters, sizeYMeters);
            }
            else if(data.shape_type == PHYSICBODY_SHAPE_TYPE.SHIP_MANIFOLD)
            {
                BodyDef_Helpers.ShipManifoldSetup(out pBody, data, gameObject, ref bodyDef, xMeters, yMeters, sizeXMeters, sizeYMeters);
            }
            else
            {
                Debug.Assert(false, "You did not set the shape type");
            }
            ////////////////
            
            // Sets a hidden pointer back to this object
            pBody.SetUserData(this);
            // Set the mass of depending on BodyDef settings.
            pBody.SetMassFromShapes();
        }

        // Gets World Position of this Physical Body (Pixels)
        public Vec2 GetWorldPosition()
        {
            Vec2 pos = pBody.GetPosition();

            return (pos * PhysicWorld.METERSTOPIXELS);
        }

        // Gets BOX2D Position (Meters)
        public Vec2 GetBox2DPosition()
        {
            return pBody.GetPosition();
        }

        // Sets position of the body 
        //      parameter: position in pixel cordinates (Will convert to Meters)
        public void SetBox2DPosition(Vec2 pixelPos)
        {
            pixelPos *= PhysicWorld.PIXELSTOMETERS;

            pBody.SetPosition(pixelPos);
        }

        // Apply Force to Physical Body.
        // Parameters:
        //      WorldForce: Direction of the force in World Space (Will convert to Meters)
        public void ApplyForce(Vec2 worldForce)
        {
            worldForce *= PhysicWorld.PIXELSTOMETERS;
            pBody.ApplyImpulse(worldForce, pBody.GetWorldCenter());
        }

        // Sets the velocity of physical body (WorldSpace)
        // Parameters:
        //      WorldVelocity: The velocity of physical body in World Space (Will convert to Meters)
        public void SetWorldVelocity(Vec2 worldVelocity)
        {
            worldVelocity *= PhysicWorld.PIXELSTOMETERS;
            pBody.SetLinearVelocity(worldVelocity);
        }

        // Sets the velocity of physical body (Box2D meters)
        // Parameters:
        //      WorldVelocity: The velocity of physical body in Box2D Space
        public void SetBox2DVelocity(Vec2 box2DVelocity)
        {
            pBody.SetLinearVelocity(box2DVelocity);
        }

        // Sets the velocity of physical body (WorldSpace)
        // Parameters:
        //      WorldVelocity: The velocity of physical body in World Space (Will convert to Meters)
        public void SetAngularVelocity(float vel)
        {
            vel *= PhysicWorld.PIXELSTOMETERS;
            pBody.SetAngularVelocity(vel);
        }

        // Sets Angle of body in Degrees (Will convert to Radians
        public void SetAngle(float degrees)
        {
            pBody.SetAngle(degrees * PhysicWorld.MATH_PI_180);
        }

        // Returns Angle of body in Radians
        public float GetAngleRads()
        {
            return pBody.GetAngle();
        }

        // Returns Angle of body in Degrees
        public float GetAngleDegs()
        {
            return pBody.GetAngle() * PhysicWorld.MATH_180_PI;
        }

        // Returns the World Velocity of the object.
        public Vec2 GetWorldVelocity()
        {
            return pBody.GetLinearVelocity() * PhysicWorld.METERSTOPIXELS;
        }

        // Returns the World Velocity of the object.
        public Vec2 GetBox2DVelocity()
        {
            return pBody.GetLinearVelocity();
        }

        // Returns the Box2D Body
        public Body GetBody()
        {
            return pBody;
        }
        


    }
}
