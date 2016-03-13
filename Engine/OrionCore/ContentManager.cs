using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Core
{
    public sealed class ContentManager
    {
        private Microsoft.Xna.Framework.Content.ContentManager _content;
        private Dictionary<string, Texture2D> _textures;
        private Dictionary<string, SpriteFont> _fonts;
        private Dictionary<string, Microsoft.Xna.Framework.Graphics.Effect> _effects;

        private static ContentManager _instance = null;
        public static ContentManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ContentManager();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }

        public GraphicsDevice Graphics
        {
            get;
            set;
        }

        internal ContentManager()
        {
            _textures = new Dictionary<string, Texture2D>();
            _fonts = new Dictionary<string, SpriteFont>();
            _effects = new Dictionary<string, Microsoft.Xna.Framework.Graphics.Effect>();
        }

        internal ContentManager(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _content = content;

            _textures = new Dictionary<string, Texture2D>();
            _fonts = new Dictionary<string, SpriteFont>();
            _effects = new Dictionary<string, Microsoft.Xna.Framework.Graphics.Effect>();
        }

        public object Load(string fileName, string refName, ContentType type)
        {
            object loadedResource = null;

            if (this.Graphics == null)
                throw new InvalidOperationException("Engine must be initialized before using the ContentManager.");

            switch(type)
            {
                case ContentType.Texture:
                    if (!_textures.ContainsKey(refName))
                        loadedResource = LoadTexture(fileName, refName);
                    break;
                case ContentType.Font:
                    if (_content == null)
                        throw new InvalidOperationException("No MonoGame Content manager available to load fonts.");

                    if (!_fonts.ContainsKey(refName))
                    {
                        SpriteFont font = _content.Load<SpriteFont>(fileName);
                        _fonts.Add(refName, font);
                        loadedResource = font;
                    }
                    break;
                case ContentType.Effect:
                    if (!_fonts.ContainsKey(refName))
                        loadedResource = LoadEffect(fileName, refName);
                    break;
            }

            return loadedResource;
        }

        public object Load(Stream stream, string refName, ContentType type)
        {
            object loadedResource = null;

            if (this.Graphics == null)
                throw new InvalidOperationException("Engine must be initialized before using the ContentManager.");

            switch (type)
            {
                case ContentType.Texture:
                    if (!_textures.ContainsKey(refName))
                        loadedResource = LoadTexture(stream, refName);
                    else
                        loadedResource = Get(refName, type);
                    break;
                case ContentType.Font:
                    throw new InvalidOperationException("Fonts cannot be loaded from a stream.");
                case ContentType.Effect:
                    if (!_effects.ContainsKey(refName))
                        loadedResource = LoadEffect(stream, refName);
                    else
                        loadedResource = Get(refName, type);
                    break;
            }

            return loadedResource;
        }

        public void Unload(string refName, ContentType type)
        {
            switch (type)
            {
                case ContentType.Texture:
                    if (_textures.ContainsKey(refName))
                    {
                        _textures[refName].Dispose();
                        _textures.Remove(refName);
                    }
                    break;
                case ContentType.Font:
                    // fonts are not heavy resources and are used often.. no need to unload
                    break;
                case ContentType.Effect:
                    break;
            }
        }

        public object Get(string refName, ContentType type)
        {
            object resource = null;

            switch (type)
            {
                case ContentType.Texture:
                    Texture2D texture = null;
                    if (_textures.TryGetValue(refName, out texture))
                        resource = texture;
                    break;
                case ContentType.Font:
                    SpriteFont font = null;
                    if (_fonts.TryGetValue(refName, out font))
                        resource = font;
                    break;
                case ContentType.Effect:
                    Microsoft.Xna.Framework.Graphics.Effect effect = null;
                    if (_effects.TryGetValue(refName, out effect))
                        resource = effect;
                    break;
            }

            return resource;
        }

        public string GetKeyForResource(object resource, ContentType type)
        {
            string key = string.Empty;

            switch (type)
            {
                case ContentType.Texture:
                    foreach (KeyValuePair<string, Texture2D> pair in _textures)
                    {
                        if (ReferenceEquals(pair.Value, resource))
                        {
                            key = pair.Key;
                            break;
                        }
                    }
                    break;
                case ContentType.Font:
                    foreach (KeyValuePair<string, SpriteFont> pair in _fonts)
                    {
                        if (ReferenceEquals(pair.Value, resource))
                        {
                            key = pair.Key;
                            break;
                        }
                    }
                    break;
                case ContentType.Effect:
                    foreach (KeyValuePair<string, Microsoft.Xna.Framework.Graphics.Effect> pair in _effects)
                    {
                        if (ReferenceEquals(pair.Value, resource))
                        {
                            key = pair.Key;
                            break;
                        }
                    }
                    break;
            }

            return key;
        }

        public bool Contains(string refName)
        {
            if (_textures.ContainsKey(refName))
                return true;

            if (_fonts.ContainsKey(refName))
                return true;

            if (_effects.ContainsKey(refName))
                return true;

            return false;
        }

        public bool Contains(string refName, ContentType type)
        {
            bool found = false;

            switch (type)
            {
                case ContentType.Texture:
                    found = _textures.ContainsKey(refName);
                    break;
                case ContentType.Font:
                    found = _fonts.ContainsKey(refName);
                    break;
                case ContentType.Effect:
                    found = _effects.ContainsKey(refName);
                    break;
            }

            return found;
        }

        private Texture2D LoadTexture(string fileName, string refName)
        {
            Texture2D texture = null;

            // Use Case: User calls load but this particular resources has already
            // been loaded.  So, check the dictionary first for our resource before
            // trying to add it as a new resource.
            if (!_textures.TryGetValue(refName, out texture))
            {
                texture = Utilities.LoadTextureFromFile(fileName, this.Graphics);
                _textures.Add(refName, texture);
            }

            return texture;
        }

        private Texture2D LoadTexture(Stream stream, string refName)
        {
            Texture2D texture = null;

            // Use Case: User calls load but this particular resources has already
            // been loaded.  So, check the dictionary first for our resource before
            // trying to add it as a new resource.
            if (!_textures.TryGetValue(refName, out texture))
            {
                texture = Utilities.LoadTextureStream(stream, this.Graphics);
                _textures.Add(refName, texture);
            }

            return texture;
        }

        private Microsoft.Xna.Framework.Graphics.Effect LoadEffect(string fileName, string refName)
        {
            Microsoft.Xna.Framework.Graphics.Effect effect = null;

            // Use Case: User calls load but this particular resources has already
            // been loaded.  So, check the dictionary first for our resource before
            // trying to add it as a new resource.
            if (!_effects.TryGetValue(refName, out effect))
            {
                effect = Utilities.LoadEffectFromFile(fileName, this.Graphics);
                _effects.Add(refName, effect);
            }

            return effect;
        }

        private Microsoft.Xna.Framework.Graphics.Effect LoadEffect(Stream stream, string refName)
        {
            Microsoft.Xna.Framework.Graphics.Effect effect = null;

            // Use Case: User calls load but this particular resources has already
            // been loaded.  So, check the dictionary first for our resource before
            // trying to add it as a new resource.
            if (!_effects.TryGetValue(refName, out effect))
            {
                effect = Utilities.LoadEffectStream(stream, this.Graphics);
                _effects.Add(refName, effect);
            }

            return effect;
        }
    }
}
