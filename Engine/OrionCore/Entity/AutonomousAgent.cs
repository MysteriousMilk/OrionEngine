using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Entity
{
    [ShowInEditor(false)]
    public class AutonomousAgent : IEntity, IUpdatable
    {
        #region AutonomousAgent Properties
        /// <summary>
        /// The speed at which the agent is moving as a vector.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The amount of force being applied to the agent's velocity each time step.
        /// </summary>
        public Vector2 Acceleration { get; set; }

        /// <summary>
        /// The max speed at which the agent can move.
        /// </summary>
        public float MaxSpeed { get; set; }

        /// <summary>
        /// The max acceleration that can be applied to the agent's velocity.
        /// </summary>
        public float MaxAcceleration { get; set; }

        /// <summary>
        /// The max deceleration force that can be applied to the agent's velocity.
        /// </summary>
        public float MaxDeceleration { get; set; }

        /// <summary>
        /// The max force that can be applied to the turning of agent.
        /// </summary>
        public float MaxTurnRate { get; set; }
        #endregion

        #region IEntity Properties
        private Vector2 _position = Vector2.Zero;

        /// <summary>
        /// The sprite's position in the world.
        /// </summary>
        public Vector2 Position { get; set; }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public void SetPosition(Vector2 pos)
        {
            _position = pos;
        }

        /// <summary>
        /// The sprite's rotation.
        /// </summary>
        public float Rotation { get; set; }

        private bool _alive;

        /// <summary>
        /// Status of the sprite.  When the alive status changes,
        /// the AliveStausChanged event is invoked.
        /// </summary>
        public bool Alive
        {
            get { return _alive; }
            set
            {
                if (value != _alive)
                {
                    _alive = value;
                    if (AliveStatusChanged != null)
                        AliveStatusChanged(this, new EventArgs());
                }
            }
        }

        public event EventHandler AliveStatusChanged;
        #endregion

        /// <summary>
        /// Returns the direction that the agent is heading in radians.
        /// </summary>
        /// <returns></returns>
        public float Heading()
        {
            return (float)Math.Atan2(this.Velocity.Y, this.Velocity.X);
        }

        /// <summary>
        /// Returns the direction that the agent is heading as a normalized vector.
        /// </summary>
        /// <returns></returns>
        public Vector2 HeadingVec()
        {
            return OrionMath.VectorNormalize(this.Velocity);
        }

        /// <summary>
        /// The update function called each time step.  It will update the position of the agent,
        /// based on it's speed and rotation.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime, IUpdatable parent)
        {
            // calc speed as a scalar
            float speed = this.Velocity.Length();

            // update the position
            this.Position += new Vector2((float)Math.Cos(OrionMath.ToRadians(this.Rotation - 90.0f)) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds,
                                         (float)Math.Sin(OrionMath.ToRadians(this.Rotation - 90.0f)) * speed * (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
