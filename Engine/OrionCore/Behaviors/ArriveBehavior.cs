using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Behaviors
{
    public class ArriveBehavior : IBehavior
    {
        public Vector2 TargetPosition { get; set; }
        public MovableEntity Agent { get; set; }
        public float SlowingRadius { get; set; }
        public float TargetRadius { get; set; }

        public ArriveBehavior(MovableEntity agent, Vector2 target, float decelerationDist)
        {
            Agent = agent;
            TargetPosition = target;
            SlowingRadius = decelerationDist;
            TargetRadius = 10.0f;
        }

        public Vector2 ComputeForce()
        {
            Vector2 desired = TargetPosition - Agent.Position;

            float distance = desired.Length();
            desired.Normalize();

            Vector2 steeringVec;
            if (distance < SlowingRadius)
            {
                float mag = OrionMath.Map(distance, 0, 100, 0, Agent.MaxSpeed);
                desired *= mag;

                steeringVec = desired - Agent.Velocity;
                steeringVec = OrionMath.VectorTruncate(steeringVec, Agent.MaxDeceleration);
            }
            else
            {
                desired *= Agent.MaxSpeed;

                steeringVec = desired - Agent.Velocity;
                steeringVec = OrionMath.VectorTruncate(steeringVec, Agent.MaxAcceleration);
            }

            return steeringVec;
        }
    }
}
