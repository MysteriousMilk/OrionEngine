using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Orion.Core
{
    public class ParticleSystemSettings
    {
        public List<Texture2D> TextureList { get; internal set; }
        public IParticleProperty<Vector2> StartVelocity { get; set; }
        public IParticleProperty<Vector2> EndVelocity { get; set; }
        public IParticleProperty<Color> StartColor { get; set; }
        public IParticleProperty<Color> EndColor { get; set; }
        public IParticleProperty<int> Alpha { get; set; }
        public IParticleProperty<float> Angle { get; set; }
        public IParticleProperty<float> AngularVelocity { get; set; }
        public IParticleProperty<float> Size { get; set; }
        public IParticleProperty<float> Life { get; set; }
        public IParticleProperty<float> SpawnFrequency { get; set; }
        public Vector2 SpawnRadius { get; set; }
        public Random Randomizer { get; set; }

        public ParticleSystemSettings(Texture2D texture)
        {
            Randomizer = new Random();

            TextureList = new List<Texture2D>();
            TextureList.Add(texture);

            StartVelocity = new Range<Vector2>(Vector2.Zero, Randomizer);
            EndVelocity = new Range<Vector2>(Vector2.Zero, Randomizer);
            StartColor = new Range<Color>(Color.White, Randomizer);
            EndColor = new Range<Color>(Color.White, Randomizer);
            Alpha = new Range<int>(255, Randomizer);
            Angle = new Range<float>(360.0f, 0.0f, Randomizer);
            AngularVelocity = new Range<float>(0.0f, Randomizer);
            Size = new Range<float>(1.0f, Randomizer);
            Life = new Range<float>(0.0f, Randomizer);
            SpawnFrequency = new Range<float>(0.0f, Randomizer);
            SpawnRadius = Vector2.Zero;
        }

        public ParticleSystemSettings(Random random, Texture2D texture)
        {
            Randomizer = random;

            TextureList = new List<Texture2D>();
            TextureList.Add(texture);

            float hue1 = (float)Randomizer.NextDouble() * 6.0f;
            float hue2 = (hue1 + ((float)Randomizer.NextDouble() * 2)) % 6f;
            Color color1 = Utilities.HSVToColor(new HueSaturationValue(hue1, 0.5f, 1));
            Color color2 = Utilities.HSVToColor(new HueSaturationValue(hue2, 0.5f, 1));

            StartVelocity = new Range<Vector2>(new Vector2(-100f, -100f), new Vector2(100f, -100f), Randomizer);
            EndVelocity = new Range<Vector2>(new Vector2(-100f, -100f), new Vector2(100f, -100f), Randomizer);
            StartColor = new Range<Color>(color1, Randomizer);
            EndColor = new Range<Color>(color2, Randomizer);
            Alpha = new Range<int>(255, 127, Randomizer);
            Angle = new Range<float>(360.0f, 0.0f, Randomizer);
            AngularVelocity = new Range<float>(0.0f, Randomizer);
            Size = new Range<float>(2.0f, 0.5f, Randomizer);
            Life = new Range<float>(1000.0f, 500.0f, Randomizer);
            SpawnFrequency = new Range<float>(10.0f, Randomizer);
            SpawnRadius = Vector2.Zero;
        }

        public ParticleSystemSettings(Random random, Texture2D texture, Vector2 startVel, Vector2 endVel, Color startColor, Color endColor,
                                      int alpha, float angle, float angVel, float size, float life, float spawnRate, Vector2 spawnRadius)
        {
            Randomizer = random;

            TextureList = new List<Texture2D>();
            TextureList.Add(texture);

            StartVelocity = new Range<Vector2>(startVel, Randomizer);
            EndVelocity = new Range<Vector2>(endVel, Randomizer);
            StartColor = new Range<Color>(startColor, Randomizer);
            EndColor = new Range<Color>(endColor, Randomizer);
            Alpha = new Range<int>(alpha, Randomizer);
            Angle = new Range<float>(angle, angle, Randomizer);
            AngularVelocity = new Range<float>(angVel, Randomizer);
            Size = new Range<float>(size, Randomizer);
            Life = new Range<float>(life, Randomizer);
            SpawnFrequency = new Range<float>(spawnRate, Randomizer);
            SpawnRadius = spawnRadius;
        }

        public ParticleSystemSettings(Random random, Texture2D texture, Range<Vector2> startVel, Range<Vector2> endVel, Range<Color> startColor,
                                      Range<Color> endColor, Range<int> alpha, Range<float> angle, Range<float> angVel, Range<float> size,
                                      Range<float> spawnRate, Vector2 spawnRadius)
        {
            Randomizer = random;

            TextureList = new List<Texture2D>();
            TextureList.Add(texture);

            StartVelocity = startVel;
            EndVelocity = endVel;
            StartColor = startColor;
            EndColor = endColor;
            Alpha = alpha;
            Angle = angle;
            AngularVelocity = angVel;
            Size = size;
            Life = Life;
            SpawnFrequency = spawnRate;
            SpawnRadius = spawnRadius;
        }

        public ParticleSystemSettings(Random random, List<Texture2D> textures, Range<Vector2> startVel, Range<Vector2> endVel, Range<Color> startColor,
                                      Range<Color> endColor, Range<int> alpha, Range<float> angle, Range<float> angVel, Range<float> size, Range<float> life,
                                      Range<float> spawnRate, Vector2 spawnRadius)
        {
            Randomizer = random;

            TextureList = textures;

            StartVelocity = startVel;
            EndVelocity = endVel;
            StartColor = startColor;
            EndColor = endColor;
            Alpha = alpha;
            Angle = angle;
            AngularVelocity = angVel;
            Size = size;
            Life = life;
            SpawnFrequency = spawnRate;
            SpawnRadius = spawnRadius;
        }

        public void SetPropertyByName(string name, IParticleProperty<float> property)
        {
            switch(name)
            {
                case "Angle":
                    Angle = property;
                    break;
                case "AngularVelocity":
                    Angle = property;
                    break;
                case "Size":
                    Size = property;
                    break;
                case "Life":
                    Life = property;
                    break;
                case "SpawnFrequency":
                    SpawnFrequency = property;
                    break;
            }
        }

        public void SetPropertyByName(string name, IParticleProperty<double> property)
        {
        }

        public void SetPropertyByName(string name, IParticleProperty<int> property)
        {
            switch (name)
            {
                case "Alpha":
                    Alpha = property;
                    break;
            }
        }

        public void SetPropertyByName(string name, IParticleProperty<Vector2> property)
        {
            switch (name)
            {
                case "StartVelocity":
                    StartVelocity = property;
                    break;
                case "EndVelocity":
                    EndVelocity = property;
                    break;
            }
        }

        public void SetPropertyByName(string name, IParticleProperty<Color> property)
        {
            switch (name)
            {
                case "StartColor":
                    StartColor = property;
                    break;
                case "EndColor":
                    EndColor = property;
                    break;
            }
        }

        public void SetPropertyByName(string name, Vector2 property)
        {
            switch (name)
            {
                case "SpawnRadius":
                    SpawnRadius = property;
                    break;
            }
        }
    }
}
