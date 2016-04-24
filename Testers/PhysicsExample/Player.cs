using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orion.Core;
using Orion.Core.Managers;
using Orion.Core.Physics;

namespace PhysicsExample
{
    public class Player : Entity
    {
        private CompoundSprite _PlayerSprite;
        private AnimatedSprite _BaseBodySprite;
        private Sprite _ArmBackSprite;
        private Sprite _ArmFrontSprite;
        private Sprite _GunSprite;
        private bool _isInverted = false;

        public Player()
        {
            SpriteDefinition spriteDef = new SpriteDefinition("PlayerBase", string.Empty, 1, 4, 64, 96);
            spriteDef.AnimationList.Add("Idle", new Animation("Idle", 0, 0, 0, false));

            _PlayerSprite = new CompoundSprite(OrionEngine.Instance.GraphicsDM.GraphicsDevice);

            _BaseBodySprite = new AnimatedSprite(spriteDef);
            _BaseBodySprite.Animate("Idle");
            _BaseBodySprite.ZOrder = 3;
            _PlayerSprite.AddSprite(_BaseBodySprite);

            _ArmBackSprite = new Sprite("ArmBack");
            _ArmBackSprite.Origin = new Vector2(68, 56);
            _ArmBackSprite.Position = new Vector2(-2, 2);
            _PlayerSprite.AddSprite(_ArmBackSprite);

            _ArmFrontSprite = new Sprite("ArmFront");
            _ArmFrontSprite.Origin = new Vector2(68, 56);
            _ArmFrontSprite.Position = new Vector2(0, 2);
            _ArmFrontSprite.ZOrder = 5;
            _PlayerSprite.AddSprite(_ArmFrontSprite);

            _GunSprite = new Sprite("Gun");
            _GunSprite.Origin = new Vector2(44, 32);
            _GunSprite.Position = new Vector2(-10, 10);
            _GunSprite.ZOrder = 4;
            _PlayerSprite.AddSprite(_GunSprite);

            Attach(_PlayerSprite);

            CircleCollider collider = OrionEngine.Instance.GetComponent<PhysicsComponent>().CreateCircleCollider(Vector2.Zero, 48, 1, ColliderType.Dynamic);
            collider.Restitution = 0.01f;
            collider.Friction = 2.0f;
            collider.UseRotation = false;
            Attach(collider);
        }

        public override void Update(GameTime gameTime, IUpdatable parent)
        {
            Vector2 mousePos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            Vector2 playerPos = Vector2.Transform(Position, OrionEngine.Instance.GetComponent<Camera2D>().Transform);

            float armRotation = OrionMath.AngleBetween(mousePos, playerPos);
            if (armRotation >= 180 && armRotation < 360)
                armRotation -= 180;

            armRotation -= 90;
            if (_isInverted)
                armRotation *= -1;

            if (armRotation >= 360)
                armRotation -= 360;
            else if (armRotation < 0)
                armRotation += 360;

            LogManager.Instance.LogMessage(this, _ArmFrontSprite, "Rotation=" + armRotation);

            _ArmBackSprite.Rotation = armRotation;
            _ArmFrontSprite.Rotation = armRotation;
            _GunSprite.Rotation = armRotation;

            ICollider collider = GetAttachable<ICollider>();

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                collider.ApplyLinearImpulse(new Vector2(-0.5f, 0.0f));

            base.Update(gameTime, parent);
        }

        public override void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            Vector2 mousePos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
            Vector2 playerPos = Vector2.Transform(Position, OrionEngine.Instance.GetComponent<Camera2D>().Transform);

            if (mousePos.X <= playerPos.X)
            {
                if (_isInverted)
                {
                    _isInverted = false;
                    _PlayerSprite.Flip();
                }
            }
            else
            {
                if (!_isInverted)
                {
                    _isInverted = true;
                    _PlayerSprite.Flip();
                }
            }

            base.Draw(spriteBatch, parent);
        }
    }
}
