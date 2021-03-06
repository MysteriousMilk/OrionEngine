﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class ParticleEmitter : GameObject, IAttachable, IDrawable, IFocusable, IUpdatable
    {
        private List<Particle> _particlePool = new List<Particle>();
        private float _elapsedTimeSinceLastSpawn = 0.0f;
        private float _elapsedTimeSinceStart = 0.0f;
        private float _nextSpawn = 0.0f;
        private float _duration = -1.0f;
        private bool _isEmitting = false;

        public ParticleSystemSettings Settings { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public Type AttachableType
        {
            get { return GetType(); }
        }

        public virtual IEnumerable<Type> Interfaces
        {
            get
            {
                yield return typeof(IAttachable);
                yield return typeof(IDrawable);
                yield return typeof(IFocusable);
                yield return typeof(ISprite);
            }
        }

        /// <summary>
        /// The draw order othe drawable item.
        /// </summary>
        public int ZOrder { get; set; }

        public bool IsEmitting
        {
            get { return _isEmitting; }
        }

        /// <summary>
        /// The blend state for the drawable.
        /// </summary>
        public BlendState BlendState { get; set; }

        public ParticleEmitter(ParticleSystemSettings settings)
        {
            Settings = settings;
            BlendState = BlendState.Additive;
            ZOrder = 0;
        }

        public void Start()
        {
            Start(-1.0f);
        }

        public void Start(float duration)
        {
            _particlePool.Clear();

            _elapsedTimeSinceLastSpawn = 0.0f;
            _elapsedTimeSinceStart = 0.0f;
            _nextSpawn = 0.0f;
            _isEmitting = true;
            _duration = duration;
        }

        public void Stop()
        {
            _isEmitting = false;
        }

        public void Update(GameTime gameTime, IUpdatable parent)
        {
            _elapsedTimeSinceLastSpawn += gameTime.ElapsedGameTime.Milliseconds;
            _elapsedTimeSinceStart += gameTime.ElapsedGameTime.Milliseconds;

            IDrawable parentDrawable = null;
            if (parent is IDrawable)
                parentDrawable = (IDrawable)parent;

            float rotation = Rotation;
            if (parentDrawable != null)
                rotation = parentDrawable.Rotation + Rotation;

            Vector2 position = GetRandomPosition(parentDrawable);

            // remove dead particles
            PruneParticlePool();

            // only create new particles while the emitter is "on" and the
            // duration has not been reached.
            if (_isEmitting && (_elapsedTimeSinceStart < _duration || _duration == -1.0f))
            {
                // time for new spawn
                if (_elapsedTimeSinceLastSpawn > _nextSpawn)
                {
                    // makes sure there is room in the pool for a new particle
                    if (_particlePool.Count < Orion.Core.Settings.Instance.MaxParticlesPerSystem)
                    {
                        _elapsedTimeSinceLastSpawn = 0.0f;
                        _nextSpawn = Settings.SpawnFrequency.GetNextValue();

                        Particle p = new Particle(
                            GetRandomTexture(),
                            position,
                            OrionMath.RotatePointPositive(Settings.StartVelocity.GetNextValue(), Vector2.Zero, rotation),
                            OrionMath.RotatePointPositive(Settings.EndVelocity.GetNextValue(), Vector2.Zero, rotation),
                            Settings.Angle.GetNextValue(),
                            Settings.AngularVelocity.GetNextValue(),
                            Settings.StartColor.GetNextValue(),
                            Settings.EndColor.GetNextValue(),
                            Settings.Alpha.GetNextValue(),
                            Settings.Size.GetNextValue(),
                            Settings.Life.GetNextValue()
                            );
                        _particlePool.Add(p);
                    }
                }
            }

            foreach (Particle p in _particlePool)
                p.Update(gameTime);
        }

        public Rectangle Bounds()
        {
            return new Rectangle(
                (int)(Position.X - Settings.SpawnRadius.X),
                (int)(Position.Y - Settings.SpawnRadius.Y),
                (int)(Settings.SpawnRadius.X * 2),
                (int)(Settings.SpawnRadius.Y * 2)
                );
        }

        public void Draw(SpriteBatch spriteBatch, IDrawable parent)
        {
            foreach (Particle p in _particlePool)
                p.Draw(spriteBatch);
        }

        private void PruneParticlePool()
        {
            _particlePool.RemoveAll(p => p.Alive == false);
        }

        private Texture2D GetRandomTexture()
        {
            int randIndex = Settings.Randomizer.Next(0, Settings.TextureList.Count);
            return Settings.TextureList[randIndex];
        }

        private Vector2 GetRandomPosition(IDrawable parent)
        {
            float rotation = Rotation;
            if (parent != null)
                rotation = parent.Rotation + Rotation;

            Vector2 relPos = OrionMath.RotatePointPositive(Position, Vector2.Zero, rotation);
            relPos += parent.Position;

            Range<float> xRange = new Range<float>(
                relPos.X + Settings.SpawnRadius.X,
                relPos.X - Settings.SpawnRadius.X,
                Settings.Randomizer);

            Range<float> yRange = new Range<float>(
                relPos.Y + Settings.SpawnRadius.Y,
                relPos.Y - Settings.SpawnRadius.Y,
                Settings.Randomizer);

            return new Vector2(xRange.GetNextValue(), yRange.GetNextValue());
        }
    }
}
