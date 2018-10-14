using Microsoft.Xna.Framework;

namespace CompositionGame.Components
{
    internal class WorldBoundsComponent : Component
    {
        public readonly int MinX, MinY, MaxX, MaxY;

        public WorldBoundsComponent(GraphicsDeviceManager graphics)
        {
            this.MinX = 0;
            this.MaxX = graphics.PreferredBackBufferWidth;
            this.MinY = 0;
            this.MaxY = graphics.PreferredBackBufferHeight;
        }
    }
}
