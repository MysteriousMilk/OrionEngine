using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public static class Utilities
    {
        public static Texture2D LoadTextureFromFile(string filename, GraphicsDevice device)
        {
            Texture2D texture = null;

            using (var stream = TitleContainer.OpenStream(filename))
            {
                texture = LoadTextureStream(stream, device);
            }

            return texture;
        }

        public static Texture2D LoadTextureStream(Stream stream, GraphicsDevice device)
		{
			Texture2D file = null;
			RenderTarget2D result = null;

			file = Texture2D.FromStream(device, stream);

			//Setup a render target to hold our final texture which will have premulitplied alpha values
			result = new RenderTarget2D(device, file.Width, file.Height);

			device.SetRenderTarget(result);
			device.Clear(Microsoft.Xna.Framework.Color.Black);

			//Multiply each color by the source alpha, and write in just the color values into the final texture
			BlendState blendColor = new BlendState();
			blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;
			blendColor.AlphaDestinationBlend = Blend.Zero;
			blendColor.ColorDestinationBlend = Blend.Zero;
			blendColor.AlphaSourceBlend = Blend.SourceAlpha;
			blendColor.ColorSourceBlend = Blend.SourceAlpha;

			SpriteBatch spriteBatch = new SpriteBatch(device);
			spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
			spriteBatch.Draw(file, file.Bounds, Microsoft.Xna.Framework.Color.White);
			spriteBatch.End();

			//Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
			BlendState blendAlpha = new BlendState();
			blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;
			blendAlpha.AlphaDestinationBlend = Blend.Zero;
			blendAlpha.ColorDestinationBlend = Blend.Zero;
			blendAlpha.AlphaSourceBlend = Blend.One;
			blendAlpha.ColorSourceBlend = Blend.One;

			spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
			spriteBatch.Draw(file, file.Bounds, Microsoft.Xna.Framework.Color.White);
			spriteBatch.End();

			//Release the GPU back to drawing to the screen
			device.SetRenderTarget(null);
			//result.Name = filename;
			return result as Texture2D;
		}

        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }

        }

        public static Microsoft.Xna.Framework.Graphics.Effect LoadEffectFromFile(string filename, GraphicsDevice device)
        {
            Microsoft.Xna.Framework.Graphics.Effect effect = null;

            using (var stream = TitleContainer.OpenStream(filename))
            {
                effect = LoadEffectStream(stream, device);
            }

            return effect;
        }

        public static Microsoft.Xna.Framework.Graphics.Effect LoadEffectStream(Stream stream, GraphicsDevice device)
        {
            Microsoft.Xna.Framework.Graphics.Effect effect = null;

            using (var reader = new BinaryReader(stream))
            {
                effect = new Microsoft.Xna.Framework.Graphics.Effect(device, ReadAllBytes(reader));
            }

            return effect;
        }

        public static Color HSVToColor(HueSaturationValue hsv)
        {
            if (hsv.H == 0 && hsv.S == 0)
                return new Color(hsv.V, hsv.V, hsv.V);

            float c = hsv.S * hsv.V;
            float x = c * (1 - Math.Abs(hsv.H % 2 - 1));
            float m = hsv.V - c;

            if (hsv.H < 1) return new Color(c + m, x + m, m);
            else if (hsv.H < 2) return new Color(x + m, c + m, m);
            else if (hsv.H < 3) return new Color(m, c + m, x + m);
            else if (hsv.H < 4) return new Color(m, x + m, c + m);
            else if (hsv.H < 5) return new Color(x + m, m, c + m);
            else return new Color(c + m, m, x + m);
        }

        public static HueSaturationValue ColorToHSV(Color color)
        {
            HueSaturationValue hsv = new HueSaturationValue();
            float min, max, delta;

            float r = (float)color.R / 255.0f;
            float g = (float)color.G / 255.0f;
            float b = (float)color.B / 255.0f;

            min = Math.Min(r, g);
            min = Math.Min(min, b);

            max = Math.Max(r, g);
            max = Math.Max(max, b);

            hsv.V = max;               // v
            delta = max - min;
            if (max != 0)
                hsv.S = delta / max;       // s
            else
            {
                // r = g = b = 0		// s = 0, v is undefined
                hsv.S = 0;
                hsv.H = -1;
                return hsv;
            }
            if (r == max)
                hsv.H = (g - b) / delta;       // between yellow & magenta
            else if (g == max)
                hsv.H = 2 + (b - r) / delta;   // between cyan & yellow
            else
                hsv.H = 4 + (r - g) / delta;   // between magenta & cyan
            hsv.H *= 60;               // degrees
            if (hsv.H < 0)
                hsv.H += 360;

            return hsv;
        }

        public static Vector2 GetPositionRelative(Vector2 center, float distance, float directionInRadians)
        {
            float yDifference = (float)Math.Sin(directionInRadians);
            float xDifference = (float)Math.Cos(directionInRadians);
            Vector2 direction = new Vector2(xDifference, yDifference);
            Vector2 precisePositionOfSatellite = center + direction * distance;
            return precisePositionOfSatellite;
        }
    }
}
