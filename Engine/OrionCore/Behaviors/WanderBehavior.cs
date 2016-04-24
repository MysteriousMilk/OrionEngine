using Microsoft.Xna.Framework;
using System;

namespace Orion.Core.Behaviors
{
    public class WanderBehavior : IBehavior
    {
        public float WanderRadius { get; set; }
        public float WanderDistance { get; set; }
        public MovableEntity Agent { get; set; }
        public Vector2 Target { get; set; }

        private float wanderTheta = 0.0f;

        public WanderBehavior(MovableEntity agent, float distance, float radius)
        {
            Agent = agent;
            WanderDistance = distance;
            WanderRadius = radius;
        }

        public Vector2 ComputeForce()
        {
            Random random = new Random();
            float min = -0.1f;
            float max = 0.1f;

            wanderTheta += (float)(random.NextDouble() * (max - min) + min);

            Vector2 circleLoc = Agent.Velocity;
            if (circleLoc.Length() > 0)
                circleLoc.Normalize();
            circleLoc *= WanderDistance;
            circleLoc += Agent.Position;

            float h = Agent.Heading();

            Vector2 circleOffset = new Vector2((float)(WanderRadius * Math.Cos(wanderTheta + h)),
                                               (float)(WanderRadius * Math.Sin(wanderTheta + h)));
            Target = circleLoc + circleOffset;

            return Seek(Target);
        }

        private Vector2 Seek(Vector2 target)
        {
            Vector2 desiredVelocity = OrionMath.VectorNormalize(target - Agent.Position) * Agent.MaxSpeed;

            Vector2 steeringVec = desiredVelocity - Agent.Velocity;
            steeringVec = OrionMath.VectorTruncate(steeringVec, Agent.MaxAcceleration);

            return steeringVec;
        }
    }
}
