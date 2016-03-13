using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core.Effect
{
    public interface IPostProcessEffect
    {
        GraphicsDevice GraphicsDevice { get; }

        void Update(GameTime gameTime);
        RenderTarget2D RenderToTexture(Texture2D input);
    }

    public enum PostProcessEffectType
    {
        Bloom = 0
    }
}
