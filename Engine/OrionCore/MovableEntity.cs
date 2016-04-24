using Microsoft.Xna.Framework;
using System;

namespace Orion.Core
{
    public class MovableEntity : Entity
    {
        /// <summary>
        /// The velocity vector of the entity.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The acceleration vector of the entity.
        /// </summary>
        public Vector2 Acceleration { get; set; }

        /// <summary>
        /// The maximum speed at which the entity can travel.
        /// </summary>
        public float MaxSpeed { get; set; }

        /// <summary>
        /// The maximum rate the entity can accelerate at.
        /// </summary>
        public float MaxAcceleration { get; set; }

        /// <summary>
        /// The maximum rate the entity can decelerate at.
        /// </summary>
        public float MaxDeceleration { get; set; }

        /// <summary>
        /// The maximum rate at which the entity can turn.
        /// </summary>
        public float MaxTurnRate { get; set; }

        /// <summary>
        /// Gets the heading of the entity in radians.
        /// </summary>
        /// <returns></returns>
        public float Heading()
        {
            return (float)Math.Atan2(Velocity.Y, Velocity.X);
        }

        /// <summary>
        /// Gets the heading of the entity as a normalized vector.
        /// </summary>
        /// <returns></returns>
        public Vector2 HeadingVec()
        {
            return OrionMath.VectorNormalize(Velocity);
        }

        public override void Update(GameTime gameTime, IUpdatable parent)
        {
            // When physics are enabled, entity movements are handled
            // by the physics engine.
            if (!_physicsFlag)
            {
                // calc speed as a scalar
                float speed = Velocity.Length();

                // update the position
                Position += new Vector2(
                    (float)Math.Cos(OrionMath.ToRadians(Rotation - 90.0f)) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds,
                    (float)Math.Sin(OrionMath.ToRadians(Rotation - 90.0f)) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds
                    );
            }

            base.Update(gameTime, parent);
        }
    }
}
