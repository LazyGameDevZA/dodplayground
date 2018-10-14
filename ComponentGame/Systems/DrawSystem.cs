using ComponentGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComponentGame.Systems
{
    public class DrawSystem
    {
        private readonly SpriteBatch spriteBatch;
        private readonly Texture2D[] texture2Ds;
        private readonly int length;
        private readonly PositionComponent[] positionComponents;
        private readonly SpriteComponent[] spriteComponents;

        public DrawSystem(
            SpriteBatch spriteBatch,
            Texture2D[] texture2Ds,
            int length,
            PositionComponent[] positionComponents,
            SpriteComponent[] spriteComponents)
        {
            this.spriteBatch = spriteBatch;
            this.texture2Ds = texture2Ds;
            this.length = length;
            this.positionComponents = positionComponents;
            this.spriteComponents = spriteComponents;
        }

        public void Draw()
        {
            this.spriteBatch.Begin();

            for(int i = 0; i < this.length; i++)
            {
                var texture2D = this.texture2Ds[this.spriteComponents[i].Index];
                var position = new Vector2(this.positionComponents[i].X, this.positionComponents[i].Y);
                var color = new Color(this.spriteComponents[i].ColorR, this.spriteComponents[i].ColorG, this.spriteComponents[i].ColorB, this.spriteComponents[i].Alpha);
                var origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
                this.spriteBatch.Draw(texture2D, position, null, color, 0.0f, origin, Vector2.One, SpriteEffects.None, 0.0f);
            }
            
            this.spriteBatch.End();
        }
    }
}
