﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core.Managers;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace Orion.Core
{
    public class Sprite : GameObject, IAttachable, IDisposableResource, IDrawable, IFocusable, ISprite
    {
        #region Fields
        /// <summary>
        /// The destination drawing rectangle for the sprite.
        /// </summary>
        protected Rectangle _drawRect;

        /// <summary>
        /// The sprite resource's reference.
        /// </summary>
        protected string _spriteRef = string.Empty;
        #endregion

        #region Sprite Properties
        /// <summary>
        /// The sprite's definition.
        /// </summary>
        public SpriteDefinition Definition { get; set; }

        /// <summary>
        /// The sprite's origin.
        /// </summary>
        public Vector2 Origin { get; set; }
        #endregion

        #region IDrawable Properties
        /// <summary>
        /// The sprite's texture.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// The scale of the sprite.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// The sprite's alpha.
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// The tint of the sprite.
        /// </summary>
        public virtual Color Tint { get; set; }

        /// <summary>
        /// The draw order of the sprite.
        /// </summary>
        public int ZOrder { get; set; }

        /// <summary>
        /// The sprite's position in the world.
        /// </summary>
        public virtual Vector2 Position { get; set; }

        /// <summary>
        /// The sprite's rotation.
        /// </summary>
        public float Rotation { get; set; }
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
                yield return typeof(IDisposableResource);
                yield return typeof(IDrawable);
                yield return typeof(IFocusable);
                yield return typeof(ISprite);
            }
        }
        #endregion

        public Sprite()
        {
            Definition = new SpriteDefinition();
            Definition.Rows = 1;
            Definition.Columns = 1;
            Definition.FrameWidth = 0;
            Definition.FrameHeight = 0;
            Definition.FrameCount = 1;

            Position = Vector2.Zero;
            Origin = Vector2.Zero;
            Rotation = 0.0f;
            Scale = 1.0f;
            Tint = Color.White;
            Alpha = 255;
            _spriteRef = string.Empty;

            ZOrder = 0;
        }

        public Sprite(string spriteRef)
        {
            Texture = (Texture2D)ContentManager.Instance.Get(spriteRef, ContentType.Texture);

            Definition = new SpriteDefinition();
            Definition.Rows = 1;
            Definition.Columns = 1;
            Definition.FrameWidth = Texture.Width;
            Definition.FrameHeight = Texture.Height;
            Definition.FrameCount = 1;
            Definition.ReferenceName = spriteRef;

            UpdateDrawRect(0);

            Position = Vector2.Zero;
            Origin = new Vector2(_drawRect.Width / 2.0f, _drawRect.Height / 2.0f);
            Rotation = 0.0f;
            Scale = 1.0f;
            Tint = Color.White;
            Alpha = 255;

            ZOrder = 0;

            _spriteRef = spriteRef;
        }

        public Sprite(SpriteDefinition spriteDef)
        {
            Definition = spriteDef;
            Texture = (Texture2D)ContentManager.Instance.Get(Definition.ReferenceName, ContentType.Texture);

            _drawRect = new Rectangle(0, 0, spriteDef.FrameWidth, spriteDef.FrameHeight);

            Position = Vector2.Zero;
            Origin = new Vector2(Definition.FrameWidth / 2.0f,
            Definition.FrameHeight / 2.0f);
            Rotation = 0.0f;
            Scale = 1.0f;
            Tint = Color.White;
            Alpha = 255;

            ZOrder = 0;

            _spriteRef = Definition.ReferenceName;
        }

        public void UpdateDefinition(SpriteDefinition def)
        {
            Definition = def;
            UpdateDefinition();
        }

        public void UpdateDefinition()
        {
            Texture = ContentManager.Instance.Get(Definition.ReferenceName, ContentType.Texture) as Texture2D;

            _drawRect = new Rectangle(0, 0, Definition.FrameWidth, Definition.FrameHeight);

            Origin = new Vector2(
                Definition.FrameWidth / 2.0f,
                Definition.FrameHeight / 2.0f
                );
        }

        public virtual void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            float rotation = Rotation;
            if (parent != null)
                rotation = parent.Rotation + Rotation;

            Vector2 finalPosition = OrionMath.RotatePointPositive(Position, Vector2.Zero, rotation);

            if (parent != null)
                finalPosition += parent.Position;

            spriteBatch.Draw(
                Texture,
                finalPosition,
                _drawRect,
                new Color(Tint, Alpha),
                (float)OrionMath.ToRadians(rotation),
                Origin,
                Scale,
                SpriteEffects.None,
                0.0f
                );
        }

        public Rectangle Bounds()
        {
            return new Rectangle(
                (int)(Position.X - Origin.X),
                (int)(Position.Y - Origin.Y),
                Definition.FrameWidth,
                Definition.FrameHeight
                );
        }

        internal void UpdateDrawRect(int frame)
        {
            int rowIndex = frame / Definition.Columns;
            int columnIndex = frame % Definition.Columns;

            int left = Definition.FrameWidth * columnIndex;
            int top = Definition.FrameHeight * rowIndex;

            _drawRect.Location = new Point(left, top);
            _drawRect.Size = new Point(Definition.FrameWidth, Definition.FrameHeight);
        }

        public override object Clone(bool preserveGuid)
        {
            Sprite clone = new Sprite(_spriteRef);
            clone.Name = this.Name;
            clone.Tag = this.Tag;
            clone.Definition = this.Definition.Clone();
            clone.Position = this.Position;
            clone.Origin = this.Origin;
            clone.Rotation = this.Rotation;
            clone.Scale = this.Scale;
            clone.Tint = this.Tint;
            clone.Alpha = this.Alpha;
            clone.ZOrder = this.ZOrder;

            if (!preserveGuid)
                clone.Id = Guid.NewGuid();

            return clone;
        }

        public static Sprite LoadFromModule(Module.Module module, XElement node)
        {
            Orion.Core.SpriteDefinition spriteDef = null;
            float x = 0;
            float y = 0;

            foreach (XAttribute attribNode in node.Attributes())
            {
                switch (attribNode.Name.LocalName)
                {
                    case "Ref":
                        spriteDef = SpriteDefinition.LoadFromModule(module, attribNode.Value);
                        break;
                }
            }

            foreach (XElement element in node.Elements())
            {
                switch(element.Name.LocalName)
                {
                    case "LocationX":
                        x = XmlConvert.ToSingle(element.Value);
                        break;
                    case "LocationY":
                        y = XmlConvert.ToSingle(element.Value);
                        break;
                }
            }

            Sprite sprite = new Sprite(spriteDef);
            sprite.Position = new Vector2(x, y);
            return sprite;
        }

        #region IDisposableResource Methods
        public List<string> GetObjectResourceReferenceList()
        {
            List<string> resourceList = new List<string>();
            resourceList.Add(Definition.ReferenceName);
            return resourceList;
        }
        #endregion
    }
}
