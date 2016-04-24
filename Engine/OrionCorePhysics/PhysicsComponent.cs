using FarseerPhysics;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace Orion.Core.Physics
{
    public class PhysicsComponent : GameComponent
    {
        internal World _World;
        private float _pixelsPerMeter;

        public float PixelsPerMeter
        {
            get { return _pixelsPerMeter; }
            set
            {
                _pixelsPerMeter = value;
                ConvertUnits.SetDisplayUnitToSimUnitRatio(value);
            }
        }

        public PhysicsComponent(Game game, Vector2 gravityVec, float pixelsPerMeter)
            : base(game)
        {
            _World = new World(gravityVec);
            PixelsPerMeter = pixelsPerMeter;
        }

        #region Collider Factory Methods
        public BoxCollider CreateBoxCollider(int width, int height, int mass, ColliderType type)
        {
            return new BoxCollider(this, Vector2.Zero, width, height, mass, type);
        }

        public BoxCollider CreateBoxCollider(Vector2 position, int width, int height, int mass, ColliderType type)
        {
            return new BoxCollider(this, position, width, height, mass, type);
        }

        public CircleCollider CreateCircleCollider(Vector2 position, int radius, int mass, ColliderType type)
        {
            return new CircleCollider(this, position, radius, mass, type);
        }
        #endregion

        #region GameComponent Overrides
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            _World.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.001));

            base.Update(gameTime);
        }
        #endregion
    }
}
