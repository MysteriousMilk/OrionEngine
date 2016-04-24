using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion.Core;
using Orion.Core.Module;
using System;
using System.Collections.Generic;

/// <summary>
/// Contract for an attachable object.
/// Attachable objects are game objects that can
/// be attached to an Entity.
/// </summary>
public interface IAttachable
{
    /// <summary>
    /// Globally unique identifier of the object.
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Name of the object given by the game engine.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// The concrete object type.
    /// </summary>
    Type AttachableType { get; }

    /// <summary>
    /// List of types that the object implements.
    /// </summary>
    IEnumerable<Type> Interfaces { get; }

    /// <summary>
    /// Performs a deep copy of the object.
    /// </summary>
    /// <returns>The new object.</returns>
    object Clone(bool preserveGuid);
}

/// <summary>
/// Contract for ai steering behaviors.
/// </summary>
public interface IBehavior
{
    /// <summary>
    /// Computes the force required for the given
    /// steering behavior.
    /// </summary>
    /// <returns></returns>
    Vector2 ComputeForce();
}

/// <summary>
/// Contract for the camera in the game engine.
/// </summary>
public interface ICamera2D
{
    /// <summary>
    /// Gets or sets the position of the camera
    /// </summary>
    /// <value>The position.</value>
    Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the move speed of the camera.
    /// The camera will tween to its destination.
    /// </summary>
    /// <value>The move speed.</value>
    float MoveSpeed { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the camera.
    /// </summary>
    /// <value>The rotation.</value>
    float Rotation { get; set; }

    /// <summary>
    /// Gets the origin of the viewport (accounts for Scale)
    /// </summary>        
    /// <value>The origin.</value>
    Vector2 Origin { get; }

    /// <summary>
    /// Gets or sets the scale of the Camera
    /// </summary>
    /// <value>The scale.</value>
    float Scale { get; set; }

    /// <summary>
    /// Gets the screen center (does not account for Scale)
    /// </summary>
    /// <value>The screen center.</value>
    Vector2 ScreenCenter { get; }

    /// <summary>
    /// Gets the transform that can be applied to 
    /// the SpriteBatch Class.
    /// </summary>
    /// <see cref="SpriteBatch"/>
    /// <value>The transform.</value>
    Matrix Transform { get; }

    /// <summary>
    /// Gets or sets the focus of the Camera.
    /// </summary>
    /// <seealso cref="IFocusable"/>
    /// <value>The focus.</value>
    IFocusable Focus { get; set; }

    /// <summary>
    /// Determines whether the target is in view given the specified position.
    /// This can be used to increase performance by not drawing objects
    /// directly in the viewport
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="texture">The texture.</param>
    /// <returns>
    ///     <c>true</c> if the target is in view at the specified position; otherwise, <c>false</c>.
    /// </returns>
    bool IsInView(Vector2 position, Texture2D texture);

    /// <summary>
    /// Determines whether the target is in view given the specified position.
    /// This can be used to increase performance by not drawing objects
    /// directly in the viewport
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="sprite">The sprite.</param>
    /// <returns>
    ///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
    /// </returns>
    bool IsInView(Vector2 position, Sprite sprite);

    /// <summary>
    /// Determines whether the target is in view given the specified position.
    /// This can be used to increase performance by not drawing objects
    /// directly in the viewport
    /// </summary>
    /// <param name="position">The position.</param>
    /// <param name="entity">The entity.</param>
    /// <returns>
    ///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
    /// </returns>
    bool IsInView(Vector2 position, Entity entity);

    /// <summary>
    /// The boundary of the camera.
    /// </summary>
    /// <returns></returns>
    Rectangle Bounds();
}

public enum ColliderType
{
    Static,
    Kinematic,
    Dynamic
}

public interface ICollider
{
    Vector2 Position { get; set; }
    Vector2 Velocity { get; set; }
    float Rotation { get; set; }
    float Mass { get; set; }
    float Friction { get; set; }
    float Restitution { get; set; }
    ColliderType Type { get; set; }
    bool UseRotation { get; set; }

    void ApplyForce(Vector2 force, Vector2 point);
    void ApplyTorque(float torque);
    void ApplyLinearImpulse(Vector2 impulse);
    void ApplyAngularImpulse(float impulse);
}

/// <summary>
/// Contract for a game component.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Reference to the current scene.
    /// </summary>
    IScene CurrentScene { get; set; }

    /// <summary>
    /// Reference to the current camera.
    /// </summary>
    /// <returns>The current camera.</returns>
    ICamera2D GetCamera();

    /// <summary>
    /// Add a game object to the scene.
    /// </summary>
    /// <param name="obj"></param>
    void AddSceneObject(GameObject obj);

    /// <summary>
    /// Loads a scene from a module.
    /// </summary>
    /// <param name="module">The module to load the scene from.</param>
    /// <param name="sceneRef">The reference to the scene in the module.</param>
    void LoadSceneFromModule(Module module, string sceneRef);
}

/// <summary>
/// Contract for a disposable resource.
/// </summary>
public interface IDisposableResource
{
    /// <summary>
    /// Gets a list a of all objects being referenced by the
    /// game object implementing this interface.
    /// </summary>
    /// <returns>List of all objects being referenced.</returns>
    List<string> GetObjectResourceReferenceList();
}

/// <summary>
/// Contract for game objects that can be drawn to the screen.
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// The position of the drawable in the world.
    /// This is relative to its parent (specified by the draw call),
    /// or absolute within the world if the parent is null.
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// The rotation of the drawable specified in degrees.
    /// </summary>
    float Rotation { get; set; }

    /// <summary>
    /// The draw order othe drawable item.
    /// </summary>
    int ZOrder { get; set; }

    /// <summary>
    /// The bounding box of the drawable.
    /// </summary>
    /// <returns>The bounding box as a rectangle</returns>
    Rectangle Bounds();

    /// <summary>
    /// Determines how the drawable is drawn to the screen.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
    /// <param name="parent">The parent drawable.  This value can be null if the drawable 
    /// does not have a parent.</param>
    void Draw(SpriteBatch spriteBatch, IDrawable parent);
}

/// <summary>
/// Contract for an Entity game object.
/// </summary>
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

/// <summary>
/// Contract for game objects that can be focused on by the camera.
/// </summary>
public interface IFocusable
{
    /// <summary>
    /// The position of the focusable object in the world.
    /// This is relative to its parent (specified by the draw call),
    /// or absolute within the world if the parent is null.
    /// </summary>
    Vector2 Position { get; }
}

/// <summary>
/// Interface for data items in the game.
/// </summary>
public interface IOrionDataObject
{
    /// <summary>
    /// Database id of the data object.
    /// </summary>
    int Id { get; set; }
}

/// <summary>
/// Contract that describes the behavior of a particle.
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface IParticleProperty<TValue>
{
    /// <summary>
    /// Gets the next value for the property.
    /// </summary>
    /// <returns>The next value.</returns>
    TValue GetNextValue();
}

/// <summary>
/// Provides a means of executing graphical operations before
/// the main draw call.
/// </summary>
public interface IPreProcessorItem
{
    /// <summary>
    /// Routine to call before the main draw call.
    /// </summary>
    void PreProcess();
}

/// <summary>
/// Effect to be applied to the screen after rendering.
/// </summary>
public interface IPostProcessEffect
{
    /// <summary>
    /// The current graphics device.
    /// </summary>
    GraphicsDevice GraphicsDevice { get; }

    /// <summary>
    /// Updates the effect.
    /// </summary>
    /// <param name="gameTime"></param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Render the effect into a texture.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    RenderTarget2D RenderToTexture(Texture2D input);
}

/// <summary>
/// Contract for a primative object.
/// </summary>
public interface IPrimitive
{
    /// <summary>
    /// The color to fill the primative with.
    /// </summary>
    Color FillColor { get; set; }

    /// <summary>
    /// The Color of the primative's border.
    /// </summary>
    Color BorderColor { get; set; }

    /// <summary>
    /// The width of the border of the primative.
    /// </summary>
    int BorderWidth { get; set; }
}

/// <summary>
/// Contract for a scene.
/// </summary>
public interface IScene
{
    /// <summary>
    /// The scene's camera.
    /// </summary>
    ICamera2D Camera { get; }

    /// <summary>
    /// List of scene variables.
    /// </summary>
    IEnumerable<GameVariable> Variables { get; }

    /// <summary>
    /// Registers a variable with the scene.
    /// </summary>
    /// <param name="variable"></param>
    void RegisterVariable(GameVariable variable);

    /// <summary>
    /// Enumerates through all objects in the scene.
    /// </summary>
    /// <returns>Enumerable of game objects.</returns>
    IEnumerable<GameObject> EnumerateScene();

    /// <summary>
    /// Adds a game object to the scene.
    /// </summary>
    /// <param name="obj">The game object.</param>
    void Add(GameObject obj);

    /// <summary>
    /// Updates all game objects in the scene.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Draws all game objects in the scene.
    /// </summary>
    void Draw();

    /// <summary>
    /// Performs a hit test on the scene.
    /// </summary>
    /// <param name="screenPos">Vector position in screen coordinates.</param>
    /// <returns>List of all game objects that were hit by the hit test.</returns>
    List<GameObject> HitTest(Vector2 screenPos);
}

/// <summary>
/// Contract for a sprite.
/// </summary>
public interface ISprite
{
    /// <summary>
    /// Globally unique identifier for the sprite.
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Name given to the game object by the game engine.
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// User defined tag for the game object.
    /// </summary>
    string Tag { get; set; }

    /// <summary>
    /// The position of the game object in the world.
    /// This is relative to its parent (specified by the draw call),
    /// or absolute within the world if the parent is null.
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// Anchor point for the game object.
    /// </summary>
    Vector2 Origin { get; set; }

    /// <summary>
    /// The rotation of the drawable specified in degrees.
    /// </summary>
    float Rotation { get; set; }

    /// <summary>
    /// Texture to apply to the game object.
    /// </summary>
    Texture2D Texture { get; set; }

    /// <summary>
    /// Definition of the sprite.
    /// </summary>
    SpriteDefinition Definition { get; set; }

    /// <summary>
    /// The draw order othe drawable item.
    /// </summary>
    int ZOrder { get; set; }

    /// <summary>
    /// The scale of the drawable item.
    /// </summary>
    float Scale { get; set; }

    /// <summary>
    /// The alpha of the drawable item.
    /// </summary>
    float Alpha { get; set; }

    /// <summary>
    /// The color to tint the drawable item.
    /// </summary>
    Color Tint { get; set; }

    /// <summary>
    /// Bounding box of the game object.
    /// </summary>
    /// <returns></returns>
    Rectangle Bounds();

    /// <summary>
    /// Determines how the drawable is drawn to the screen.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch that this drawable will be drawn under.</param>
    /// <param name="parent">The parent drawable.  This value can be null if the drawable 
    /// does not have a parent.</param>
    void Draw(SpriteBatch spriteBatch, IDrawable parent);

    /// <summary>
    /// Performs a deep copy of the game object.
    /// </summary>
    /// <returns>The clone object.</returns>
    object Clone(bool preserveGuid);
}

/// <summary>
/// Contract for game objects that receive updates.
/// </summary>
public interface IUpdatable
{
    /// <summary>
    /// Updates the game object.
    /// </summary>
    /// <param name="gameTime">The current game time.</param>
    /// <param name="parent">The game object's parent (can be null).</param>
    void Update(GameTime gameTime, IUpdatable parent);
}

public struct Dimension
{
    public float Width;
    public float Height;

    public Dimension(float width, float height)
    {
        Width = width;
        Height = height;
    }
}