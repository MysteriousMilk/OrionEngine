using Microsoft.Xna.Framework;

namespace Orion.Core.Behaviors
{
    public class PursuitBehavior : IBehavior
    {
        public MovableEntity Agent { get; set; }
        public MovableEntity Evader { get; set; }
        public Vector2 FuturePosition { get; set; }

        public PursuitBehavior(MovableEntity agent, MovableEntity evader)
        {
            Agent = agent;
            Evader = evader;
        }

        public Vector2 ComputeForce()
        {
            float distance = (Evader.Position - Agent.Position).Length();
            float lookAhead =distance / Agent.MaxSpeed;
            FuturePosition = Evader.Position + (lookAhead * Evader.Velocity);
            return Seek(FuturePosition);
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
