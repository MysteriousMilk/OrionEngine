using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Entity
{
    public interface IEntity
    {
        /// <summary>
        /// The position of the entity.
        /// </summary>
        Vector2 Position { get; set; }
        
        /// <summary>
        /// The entity's rotation in degrees.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Whether the entity is still alive/valid or not.
        /// </summary>
        bool Alive { get; set; }
    }
}
