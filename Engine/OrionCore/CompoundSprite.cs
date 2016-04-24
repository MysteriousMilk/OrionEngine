using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class CompoundSprite : GameObject, IAttachable, IDrawable, IFocusable, IUpdatable
    {
        private List<Sprite> _spriteList;
        private RenderTarget2D _renderTarget;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private SpriteEffects _spriteEffects;

        #region IDrawable Properties
        /// <summary>
        /// The position of the drawable in the world.
        /// This is relative to its parent (specified by the draw call),
        /// or absolute within the world if the parent is null.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The rotation of the drawable specified in degrees.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The draw order othe drawable item.
        /// </summary>
        public int ZOrder { get; set; }
        #endregion

        #region IAttachable Properties
        /// <summary>
        /// Returns the type of attachable.
        /// </summary>
        public Type AttachableType
        {
            get { return GetType(); }
        }

        /// <summary>
        /// Returns a collection of all interfaces that the
        /// attachable object implements.
        /// </summary>
        public virtual IEnumerable<Type> Interfaces
        {
            get
            {
                yield return typeof(IAttachable);
                yield return typeof(IDrawable);
                yield return typeof(IFocusable);
                yield return typeof(IUpdatable);
            }
        }
        #endregion

        public CompoundSprite(GraphicsDevice _graphics)
        {
            _graphicsDevice = _graphics;
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _spriteEffects = SpriteEffects.None;

            _spriteList = new List<Sprite>();
        }

        public void AddSprite(Sprite sprite)
        {
            _spriteList.Add(sprite);
            CreateRenderTarget();
        }

        public void RemoveSprite(Sprite sprite)
        {
            _spriteList.Remove(sprite);
            CreateRenderTarget();
        }

        /// <summary>
        /// The bounding box of the drawable.
        /// </summary>
        /// <returns>The bounding box as a rectangle</returns>
        public Rectangle Bounds()
        {
            Vector2 topLeft = Vector2.Zero;
            Vector2 bottomRight = Vector2.Zero;

            foreach (Sprite sprite in _spriteList)
            {
                Rectangle bounds = sprite.Bounds();

                if (topLeft == Vector2.Zero)
                {
                    topLeft = new Vector2(bounds.X, bounds.Y);
                }
                else
                {
                    if (bounds.X < topLeft.X)
                        topLeft.X = bounds.X;
                    if (bounds.Y < topLeft.Y)
                        topLeft.Y = bounds.Y;
                }

                Vector2 boundsBottomRight = new Vector2(bounds.X + bounds.Width, bounds.Y + bounds.Height);
                if (bottomRight == Vector2.Zero)
                {
                    bottomRight = new Vector2(boundsBottomRight.X, boundsBottomRight.Y);
                }
                else
                {
                    if (boundsBottomRight.X > bottomRight.X)
                        bottomRight.X = boundsBottomRight.X;
                    if (boundsBottomRight.Y > bottomRight.Y)
                        bottomRight.Y = boundsBottomRight.Y;
                }
            }

            // bounds calculation was done in relative space
            // alter final location based on entity position
            topLeft += Position;
            bottomRight += Position;

            int width = (int)(bottomRight.X - topLeft.X);
            int height = (int)(bottomRight.Y - topLeft.Y);
            return new Rectangle((int)topLeft.X, (int)topLeft.Y, width, height);
        }

        public void Flip()
        {
            if (_spriteEffects == SpriteEffects.None)
                _spriteEffects = SpriteEffects.FlipHorizontally;
            else if (_spriteEffects == SpriteEffects.FlipHorizontally)
                _spriteEffects = SpriteEffects.None;
        }

        /// <summary>
        /// Updates the game object.
        /// </summary>
        /// <param name="gameTime">The current game time.</param>
        /// <param name="parent">The game object's parent (can be null).</param>
        public void Update(GameTime gameTime, IUpdatable parent)
        {
            RebuildTexture();
        }

        /// <summary>
        /// Determines how the drawable is drawn to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
        /// <param name="parent">The parent drawable.  This value can be null if the drawable 
        /// does not have a parent.</param>
        public void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            float rotation = Rotation;
            if (parent != null)
                rotation = parent.Rotation + Rotation;

            Vector2 finalPosition = OrionMath.RotatePointPositive(Position, Vector2.Zero, rotation);
            finalPosition += parent.Position;

            if (_renderTarget != null)
            {
                spriteBatch.Draw(
                    _renderTarget,
                    finalPosition,
                    new Rectangle(0, 0, _renderTarget.Width, _renderTarget.Height),
                    Color.White,
                    (float)OrionMath.ToRadians(rotation),
                    new Vector2(_renderTarget.Width / 2, _renderTarget.Height / 2),
                    1.0f,
                    _spriteEffects,
                    0.0f
                    );
            }
        }

        private void CreateRenderTarget()
        {
            if (_renderTarget != null)
                _renderTarget.Dispose();
            _renderTarget = null;

            Rectangle bounds = Bounds();
            _renderTarget = new RenderTarget2D(_graphicsDevice, bounds.Width, bounds.Height);
        }

        private void RebuildTexture()
        {
            _graphicsDevice.SetRenderTarget(_renderTarget);
            _graphicsDevice.Clear(Color.Transparent);

            Rectangle bounds = Bounds();
            Matrix transform = Matrix.Identity * Matrix.CreateTranslation(bounds.Width / 2, bounds.Height / 2, 0);

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, transform);

            foreach (Sprite sprite in _spriteList.OrderBy(sprite => sprite.ZOrder))
                sprite.Draw(_spriteBatch, null);

            _spriteBatch.End();

            _graphicsDevice.SetRenderTarget(null);
        }
    }
}
