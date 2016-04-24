using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orion.Core
{
    [ShowInEditor(false)]
    public class Entity : GameObject, IAttachable, IDrawable, IEntity, IFocusable, IUpdatable
    {
        #region Fields
        private Vector2 _position;
        private float _rotation;
        protected bool _physicsFlag = false;
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
                yield return typeof(IEntity);
                yield return typeof(IFocusable);
                yield return typeof(IUpdatable);
            }
        }
        #endregion

        public List<IAttachable> AttachedObjects
        {
            get;
            protected set;
        }

        /// <summary>
        /// The position of the entity.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                if (_physicsFlag)
                {
                    ICollider c = GetAttachable<ICollider>();
                    return c.Position;
                }
                return _position;
            }
            set
            {
                if (_physicsFlag)
                {
                    ICollider c = GetAttachable<ICollider>();
                    c.Position = value;
                }
                else
                {
                    _position = value;
                }
            }
        }

        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        public float Rotation
        {
            get
            {
                if (_physicsFlag)
                {
                    ICollider c = GetAttachable<ICollider>();

                    if (c.UseRotation)
                        return c.Rotation;
                }
                return _rotation;
            }
            set
            {
                if (_physicsFlag)
                {
                    ICollider c = GetAttachable<ICollider>();

                    if (c.UseRotation)
                    {
                        c.Rotation = value;
                        return;
                    }
                }

                _rotation = value;
            }
        }

        /// <summary>
        /// Whether the entity is still alive/valid or not.
        /// </summary>
        public bool Alive { get; set; }

        /// <summary>
        /// The draw order othe drawable item.
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// Draw effect for the drawable.
        /// This only effects texture based drawables.
        /// </summary>
        public SpriteEffects SpriteEffect { get; set; }

        public Entity()
        {
            Alive = true;
            ZOrder = 0;

            AttachedObjects = new List<IAttachable>();
        }

        public void Attach(IAttachable attachable)
        {
            if (!AttachedObjects.Contains(attachable))
                AttachedObjects.Add(attachable);

            if (attachable is ICollider)
                _physicsFlag = true;
        }

        public void Detach(IAttachable attachable)
        {
            AttachedObjects.Remove(attachable);
        }

        public IAttachable GetAttachableByName(string name)
        {
            return AttachedObjects.Where(obj => obj.Name == name).FirstOrDefault();
        }

        public TAttachable GetAttachable<TAttachable>()
        {
            Type type = typeof(TAttachable);
            IAttachable attachable = GetAttachableByType(type);

            return (TAttachable)attachable;
        }

        public IAttachable GetAttachableByType(Type attachableType)
        {
            return GetAttachables(attachableType).FirstOrDefault();
        }

        public IAttachable GetAttachableByType(string typeName)
        {
            return GetAttachables(typeName).FirstOrDefault();
        }

        public IEnumerable<TAttachable> GetAttachables<TAttachable>()
        {
            Type type = typeof(TAttachable);

            foreach (IAttachable attachable in GetAttachables(type))
                yield return (TAttachable)attachable;
        }

        public IEnumerable<IAttachable> GetAttachables(Type attachableType)
        {
            return GetAttachables(attachableType.Name);
        }

        public IEnumerable<IAttachable> GetAttachables(string typeName)
        {
            foreach (IAttachable attachable in AttachedObjects.Where(obj => obj.AttachableType.Name.Equals(typeName)))
                yield return attachable;

            foreach (IAttachable attachable in AttachedObjects)
            {
                if (attachable.Interfaces.Count(type => type.Name.Equals(typeName)) > 0)
                    yield return attachable;
            }
        }

        //public IDrawable GetAttachedDrawable()
        //{
        //    return (IDrawable)GetAttachableByType(typeof(IDrawable));
        //}

        //public IOrionDataObject GetAttachedDataObject()
        //{
        //    return (IOrionDataObject)GetAttachableByType(typeof(IOrionDataObject));
        //}

        public virtual Rectangle Bounds()
        {
            Vector2 topLeft = Vector2.Zero;
            Vector2 bottomRight = Vector2.Zero;

            foreach (IDrawable drawable in EnumerateDrawables().OrderBy(drawable => drawable.ZOrder))
            {
                Rectangle bounds = drawable.Bounds();

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

        public void Flip(SpriteEffects flipDirection)
        {
            if ((SpriteEffect == SpriteEffects.None && flipDirection == SpriteEffects.FlipHorizontally) ||
                (SpriteEffect == SpriteEffects.FlipHorizontally && flipDirection == SpriteEffects.None))
            {
                foreach (IDrawable drawable in EnumerateDrawables().OrderBy(drawable => drawable.ZOrder))
                {
                    //Vector2 invertVec = new Vector2(-1.0f, 0.0f);
                    //drawable.SpriteEffect = flipDirection;
                    //drawable.Position *= invertVec;
                }
            }

            SpriteEffect = flipDirection;
        }

        public virtual void Update(GameTime gameTime, IUpdatable parent)
        {
            foreach (IUpdatable updatable in EnumerateUpdatables())
                updatable.Update(gameTime, this);
        }

        public virtual void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            foreach (IDrawable drawable in EnumerateDrawables().OrderBy(drawable => drawable.ZOrder))
            {
                drawable.Draw(spriteBatch, this);
            }
        }

        private IEnumerable<IDrawable> EnumerateDrawables()
        {
            foreach(IAttachable attachable in AttachedObjects.Where(attachable => attachable is IDrawable))
            {
                if (attachable is IDrawable)
                    yield return (IDrawable)attachable;
            }
        }

        private IEnumerable<IUpdatable> EnumerateUpdatables()
        {
            foreach (IAttachable attachable in AttachedObjects.Where(attachable => attachable is IUpdatable))
            {
                if (attachable is IUpdatable)
                    yield return (IUpdatable)attachable;
            }
        }
    }
}
