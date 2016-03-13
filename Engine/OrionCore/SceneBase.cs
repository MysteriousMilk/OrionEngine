﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public class SceneBase : OrionObject, IScene, IDisposable
    {
        /// <summary>
        /// Scene Graph - separates sprites by ZOrder.
        /// </summary>
        private Dictionary<int, List<OrionObject>> _sceneGraph;

        /// <summary>
        /// Used to keep the zorder sorted.
        /// Note: possibly come back and change the scene graph
        /// to a sorted dictionary.
        /// </summary>
        private List<int> _zorderList;

        /// <summary>
        /// List of resources being used by the scene.
        /// This list will be used to unallocate resource when the scene is removed.
        /// </summary>
        private List<string> _sceneResourceList;

        /// <summary>
        /// The sprite batch used to draw the scene.
        /// </summary>
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// The game's graphics device.
        /// </summary>
        protected GraphicsDevice _graphics;

        /// <summary>
        /// Reference to the current camera.
        /// </summary>
        public ICamera2D Camera
        {
            get;
            private set;
        }

        public SceneBase(GraphicsDevice graphics, ICamera2D camera)
        {
            _sceneGraph = new Dictionary<int, List<OrionObject>>();
            _zorderList = new List<int>();

            _spriteBatch = new SpriteBatch(graphics);
            _graphics = graphics;
            Camera = camera;
        }

        public IEnumerable<OrionObject> EnumerateScene()
        {
            foreach (int zorder in _zorderList)
            {
                foreach (OrionObject obj in _sceneGraph[zorder])
                {
                    yield return obj;
                }
            }
        }

        public void Add(OrionObject obj)
        {
            int zorder = 0;

            if (obj is IDrawable)
            {
                // get the zorder for the sprite
                zorder = (obj as IDrawable).ZOrder;
            }

            // check to see if we already have a list of sprites
            // for this zorder
            if(_sceneGraph.ContainsKey(zorder))
            {
                List<OrionObject> spriteList = null;
                if (_sceneGraph.TryGetValue(zorder, out spriteList))
                    spriteList.Add(obj);
            }
            else
            {
                // if not, we need to create one
                List<OrionObject> spriteList = new List<OrionObject>();
                spriteList.Add(obj);
                _sceneGraph.Add(zorder, spriteList);
            }

            _zorderList = _sceneGraph.Keys.ToList();
            _zorderList.Sort();
        }

        public virtual void Update(GameTime gameTime)
        {
            List<OrionObject> toRemove = new List<OrionObject>();

            foreach (int zorder in _zorderList)
            {
                foreach (OrionObject obj in _sceneGraph[zorder])
                {
                    if (obj is IUpdatable)
                        (obj as IUpdatable).Update(gameTime, null);

                    if (obj is IEntity)
                    {
                        if (!(obj as IEntity).Alive)
                            toRemove.Add(obj);
                    }
                }
            }

            foreach (OrionObject obj in toRemove)
                RemoveSprite(obj);
        }

        public virtual void Draw()
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.Transform);
            foreach (int zorder in _zorderList)
            {
                foreach (OrionObject obj in _sceneGraph[zorder])
                {
                    if (obj is IDrawable)
                    {
                        // Particle Emitters can have different blend states, so that must
                        // be accounted for.
                        if (obj is ParticleEmitter)
                        {
                            BlendState currBlendState = _spriteBatch.GraphicsDevice.BlendState;
                            ParticleEmitter emitter = (ParticleEmitter)obj;

                            if (emitter.BlendState != currBlendState)
                            {
                                _spriteBatch.End();

                                _spriteBatch.Begin(SpriteSortMode.Deferred, emitter.BlendState, null, null, null, null, Camera.Transform);
                                emitter.Draw(_spriteBatch, null);
                                _spriteBatch.End();

                                _spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.Transform);
                            }
                            else
                                emitter.Draw(_spriteBatch, null);
                        }
                        else
                            (obj as IDrawable).Draw(_spriteBatch, null);
                    }
                }
            }
            _spriteBatch.End();
        }

        public List<OrionObject> HitTest(Vector2 screenPos)
        {
            List<OrionObject> positiveHits = new List<OrionObject>();
            Vector2 worldPos = Vector2.Transform(screenPos, Matrix.Invert(this.Camera.Transform));

            foreach (int zorder in _zorderList)
            {
                foreach (OrionObject obj in _sceneGraph[zorder])
                {
                    if (obj is IDrawable)
                    {
                        if ((obj as IDrawable).Bounds().Contains(worldPos))
                            positiveHits.Add(obj);
                    }
                }
            }

            return positiveHits;
        }

        private void RemoveSprite(OrionObject obj)
        {
            int zorder = 0;
            if (obj is IDrawable)
                zorder = (obj as IDrawable).ZOrder;

            List<OrionObject> spriteList = null;

            if (_sceneGraph.TryGetValue(zorder, out spriteList))
            {
                spriteList.Remove(obj);
                System.Diagnostics.Debug.WriteLine("Scene Object Removed.");
            }
        }

        public virtual void Dispose()
        {
            foreach (int zorder in _zorderList)
            {
                foreach (OrionObject obj in _sceneGraph[zorder])
                {
                    if (obj is IDisposableResource)
                    {
                        foreach (string reference in (obj as IDisposableResource).GetObjectResourceReferenceList())
                            ContentManager.Instance.Unload(reference, ContentType.Texture);
                    }
                }
            }
        }
    }
}