using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Orion.Core.Physics
{
    public abstract class Collider
    {
        protected Body _body;

        public Vector2 Position
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(_body.Position);
            }
            set
            {
                _body.Position = ConvertUnits.ToSimUnits(value);
            }
        }

        public Vector2 Velocity
        {
            get
            {
                return ConvertUnits.ToDisplayUnits(_body.LinearVelocity);
            }
            set
            {
                _body.LinearVelocity = ConvertUnits.ToSimUnits(value);
            }
        }

        public float Rotation
        {
            get
            {
                return (float)OrionMath.ToDegrees(_body.Rotation);
            }
            set
            {
                _body.Rotation = (float)OrionMath.ToRadians(value);
            }
        }

        public float Mass
        {
            get
            {
                return _body.Mass;
            }
            set
            {
                _body.Mass = value;
                _body.ResetMassData();
            }
        }

        public float Friction
        {
            get
            {
                return _body.Friction;
            }
            set
            {
                _body.Friction = value;
            }
        }

        public float Restitution
        {
            get
            {
                return _body.Restitution;
            }
            set
            {
                _body.Restitution = value;
            }
        }

        public ColliderType Type
        {
            get
            {
                if (_body.BodyType == BodyType.Static)
                    return ColliderType.Static;
                else if (_body.BodyType == BodyType.Dynamic)
                    return ColliderType.Dynamic;
                else
                    return ColliderType.Kinematic;
            }
            set
            {
                if (value == ColliderType.Static)
                    _body.BodyType = BodyType.Static;
                else if (value == ColliderType.Dynamic)
                    _body.BodyType = BodyType.Dynamic;
                else
                    _body.BodyType = BodyType.Kinematic;
            }
        }

        public bool UseRotation
        {
            get;
            set;
        }

        internal Collider()
        {
            UseRotation = true;
        }

        public void ApplyForce(Vector2 force, Vector2 point)
        {
            _body.ApplyForce(force, point);
        }

        public void ApplyTorque(float torque)
        {
            _body.ApplyTorque(torque);
        }

        public void ApplyLinearImpulse(Vector2 impulse)
        {
            _body.ApplyLinearImpulse(impulse);
        }

        public void ApplyAngularImpulse(float impulse)
        {
            _body.ApplyAngularImpulse(impulse);
        }
    }
}
