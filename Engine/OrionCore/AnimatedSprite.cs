using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Orion.Core.Event;

namespace Orion.Core
{
    [ShowInEditor(true)]
    public class AnimatedSprite : Sprite, IUpdatable
    {
        #region CurrentAnimation
        private Animation _currentAnimation;

        /// <summary>
        /// The current animation of the sprite.
        /// </summary>
        public Animation CurrentAnimation
        {
            get { return _currentAnimation; }
            set { _currentAnimation = value; }
        }

        public override IEnumerable<Type> Interfaces
        {
            get
            {
                foreach (Type type in base.Interfaces)
                    yield return type;
                yield return typeof(IUpdatable);
            }
        }
        #endregion

        public event AnimationStateEventHandler AnimationStateChanged;

        public AnimatedSprite(string spriteRef)
            : base(spriteRef)
        {

        }

        public AnimatedSprite(SpriteDefinition spriteDef)
            : base(spriteDef)
        {

        }

        public void Animate(string animationTag)
        {
            // stop listening to the old animation
            if (_currentAnimation != null)
                _currentAnimation.AnimationStateChanged -= OnAnimationStateChanged;

            // get the new animation
            if (Definition.AnimationList.TryGetValue(animationTag, out _currentAnimation))
            {
                _currentAnimation.AnimationStateChanged += OnAnimationStateChanged;
                _currentAnimation.Play();
            }
        }

        public void Update(GameTime gameTime, IUpdatable parent)
        {
            int frame = 0;
            if (_currentAnimation != null)
                frame = _currentAnimation.CurrentFrame;

            UpdateDrawRect(frame);

            // step the animation
            if (_currentAnimation != null)
                _currentAnimation.Update(gameTime);
        }

        private void OnAnimationStateChanged(object sender, AnimationStateEventArgs args)
        {
            if (AnimationStateChanged != null)
                AnimationStateChanged(sender, args);
        }
    }
}
