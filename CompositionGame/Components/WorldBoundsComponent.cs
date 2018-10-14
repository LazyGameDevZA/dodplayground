using Microsoft.Xna.Framework;

namespace CompositionGame.Components
{
    internal class WorldBoundsComponent : Component
    {
        private readonly GraphicsDeviceManager graphics;
        
        public int MinX, MinY, MaxX, MaxY;

        public WorldBoundsComponent(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public override void Start()
        {
            this.MinX = 0;
            this.MaxX = graphics.PreferredBackBufferWidth;
            this.MinY = 0;
            this.MaxY = graphics.PreferredBackBufferHeight;
        }
    }
}
