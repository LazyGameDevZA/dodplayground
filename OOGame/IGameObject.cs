using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ObjectOriented
{
    internal interface IGameObject
    {
        void Initialize(GraphicsDeviceManager graphics);

        void LoadContent(SpriteBatch spriteBatch, Texture2D texture2D);
        
        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
