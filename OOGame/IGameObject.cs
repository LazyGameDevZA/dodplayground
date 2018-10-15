using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ObjectOriented
{
    internal interface IGameObject
    {
        Color SpriteColor { get; }
        int SpriteIndex { get; }
        Vector2 Position { get; }

        void Initialize(GraphicsDeviceManager graphics);

        void Update(GameTime gameTime);
    }
}
