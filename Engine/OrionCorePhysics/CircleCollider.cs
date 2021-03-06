﻿using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Orion.Core.Physics
{
    public class CircleCollider : Collider, IAttachable, ICollider
    {
        #region IAttachable Properties
        public Guid Id { get; set; }

        public string Name { get; set; }

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
                yield return typeof(ICollider);
            }
        }
        #endregion

        internal CircleCollider(PhysicsComponent sim, Vector2 position, int radius, int mass, ColliderType type)
        {
            BodyType bodyType = BodyType.Kinematic;
            if (type == ColliderType.Static)
                bodyType = BodyType.Static;
            else if (type == ColliderType.Dynamic)
                bodyType = BodyType.Dynamic;

            _body = BodyFactory.CreateCircle(sim._World, ConvertUnits.ToSimUnits(radius), mass, ConvertUnits.ToSimUnits(position), 0, bodyType);
            Restitution = 0.3f;
            Friction = 0.5f;
        }

        public object Clone(bool preserveGuid)
        {
            CircleCollider clone = (CircleCollider)MemberwiseClone();
            clone._body = _body.DeepClone();

            if (!preserveGuid)
                clone.Id = Guid.NewGuid();

            return clone;
        }
    }
}
