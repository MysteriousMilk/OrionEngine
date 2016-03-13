using Microsoft.Xna.Framework;
using Orion.Core.Event;

namespace Orion.Core.Entity
{
    [ShowInEditor(true)]
    public class AnimatedEntity : EntityBase
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
        #endregion

        public event AnimationStateEventHandler AnimationStateChanged;

        public AnimatedEntity(string spriteRef)
            : base()
        {
            this.Sprite = new Sprite(spriteRef);
        }

        public AnimatedEntity(SpriteDefinition spriteDef)
            : base()
        {
            this.Sprite = new Sprite(spriteDef);
        }

        public AnimatedEntity(Sprite sprite)
        {
            this.Sprite = sprite;
        }

        public void Animate(string animationTag)
        {
            // stop listening to the old animation
            if (_currentAnimation != null)
                _currentAnimation.AnimationStateChanged -= OnAnimationStateChanged;

            // get the new animation
            if (this.Sprite.Definition.AnimationList.TryGetValue(animationTag, out _currentAnimation))
            {
                _currentAnimation.AnimationStateChanged += OnAnimationStateChanged;
                _currentAnimation.Play();
            }
        }

        public override void Update(GameTime gameTime, IUpdatable parent)
        {
            int frame = 0;
            if (_currentAnimation != null)
                frame = _currentAnimation.CurrentFrame;

            this.Sprite.UpdateDrawRect(frame);

            // step the animation
            if (_currentAnimation != null)
                _currentAnimation.Update(gameTime);

            base.Update(gameTime, parent);
        }

        private void OnAnimationStateChanged(object sender, AnimationStateEventArgs args)
        {
            if (this.AnimationStateChanged != null)
                AnimationStateChanged(sender, args);
        }
    }
}
