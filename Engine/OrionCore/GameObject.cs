using System;

namespace Orion.Core
{
    public abstract class GameObject
    {
        /// <summary>
        /// Globally unique identifier for the game object.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name delegated to the game object by the game engine.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User defined tag given to the game object by the user.
        /// </summary>
        public string Tag { get; set; }

        public GameObject()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Tag = string.Empty;
        }

        public virtual object Clone(bool preserveGuid)
        {
            GameObject clone = MemberwiseClone() as GameObject;
            if (!preserveGuid)
                clone.Id = Guid.NewGuid();
            return clone;
        }
    }
}
