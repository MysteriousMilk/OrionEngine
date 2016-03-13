using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Entity
{
    [ShowInEditor(false)]
    public abstract class EntityBase : OrionObject, IDrawable, IEntity, IFocusable, IUpdatable
    {
        public List<IAttachableObject> AttachedObjects
        {
            get;
            protected set;
        }

        /// <summary>
        /// The graphical representation of the entity.
        /// </summary>
        public Sprite Sprite { get; set; }

        /// <summary>
        /// The position of the entity.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Whether the entity is still alive/valid or not.
        /// </summary>
        public bool Alive { get; set; }

        /// <summary>
        /// Velocity vector for the entity.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// The draw order othe drawable item.
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// Data associated with the entity.
        /// </summary>
        //public IOrionDataObject Data { get; set; }

        public EntityBase()
        {
            Alive = true;
            Velocity = Vector2.Zero;
            ZOrder = 0;

            AttachedObjects = new List<IAttachableObject>();
        }

        public void Attach(IAttachableObject attachable)
        {
            if (!AttachedObjects.Contains(attachable))
                AttachedObjects.Add(attachable);
        }

        public void Detach(IAttachableObject attachable)
        {
            AttachedObjects.Remove(attachable);
        }

        public IAttachableObject GetAttachableByName(string name)
        {
            return AttachedObjects.Where(obj => obj.Name == name).FirstOrDefault();
        }

        public TAttachable GetAttachable<TAttachable>()
        {
            Type type = typeof(TAttachable);
            IAttachableObject attachable = GetAttachableByType(type);

            return (TAttachable)attachable;
        }

        public IAttachableObject GetAttachableByType(Type attachableType)
        {
            return GetAttachables(attachableType).FirstOrDefault();
        }

        public IAttachableObject GetAttachableByType(string typeName)
        {
            return GetAttachables(typeName).FirstOrDefault();
        }

        public IEnumerable<TAttachable> GetAttachables<TAttachable>()
        {
            Type type = typeof(TAttachable);

            foreach (IAttachableObject attachable in GetAttachables(type))
                yield return (TAttachable)attachable;
        }

        public IEnumerable<IAttachableObject> GetAttachables(Type attachableType)
        {
            return GetAttachables(attachableType.Name);
        }

        public IEnumerable<IAttachableObject> GetAttachables(string typeName)
        {
            foreach (IAttachableObject attachable in AttachedObjects.Where(obj => obj.AttachableType.Name.Equals(typeName)))
                yield return attachable;

            foreach (IAttachableObject attachable in AttachedObjects)
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

        public virtual void Update(GameTime gameTime, IUpdatable parent)
        {
            Position += Velocity;

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
            foreach(IAttachableObject attachable in AttachedObjects.Where(attachable => attachable is IDrawable))
            {
                if (attachable is IDrawable)
                    yield return (IDrawable)attachable;
            }
        }

        private IEnumerable<IUpdatable> EnumerateUpdatables()
        {
            foreach (IAttachableObject attachable in AttachedObjects.Where(attachable => attachable is IUpdatable))
            {
                if (attachable is IUpdatable)
                    yield return (IUpdatable)attachable;
            }
        }
    }
}
