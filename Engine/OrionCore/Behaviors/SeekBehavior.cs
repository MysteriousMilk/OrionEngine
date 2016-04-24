using Microsoft.Xna.Framework;

namespace Orion.Core.Behaviors
{
    public class SeekBehavior : IBehavior
    {
        public Vector2 TargetPosition { get; set; }
        public MovableEntity Agent { get; set; }

        public SeekBehavior(MovableEntity agent, Vector2 target)
        {
            Agent = agent;
            TargetPosition = target;
        }

        public Vector2 ComputeForce()
        {
            Vector2 desiredVelocity = OrionMath.VectorNormalize(TargetPosition - Agent.Position) * Agent.MaxSpeed;

            Vector2 steeringVec = desiredVelocity - Agent.Velocity;
            steeringVec = OrionMath.VectorTruncate(steeringVec, Agent.MaxAcceleration);

            return steeringVec;
        }
    }
}
