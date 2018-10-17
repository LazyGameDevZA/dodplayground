using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.PerformanceMonitoring
{
    public static class PerfMon
    {
        private static Stopwatch initialize;
        private static Stopwatch update;
        private static Stopwatch draw;

        private static SpriteBatch spriteBatch;
        private static SpriteFont spriteFont;
        private static Texture2D dataBackdrop;

        public static void InitializeStarted()
        {
            initialize = Stopwatch.StartNew();
        }

        public static void InitializeFinished(SpriteBatch batch, SpriteFont font)
        {
            initialize.Stop();
            spriteBatch = batch;
            spriteFont = font;
            dataBackdrop = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            dataBackdrop.SetData(new[] { Color.White });
        }

        public static void UpdateStarted()
        {
            update = Stopwatch.StartNew();
        }

        public static void UpdateFinished()
        {
            update.Stop();
        }

        public static void DrawStarted()
        {
            draw = Stopwatch.StartNew();
        }

        public static void DrawFinished()
        {
            draw.Stop();

            spriteBatch.Begin();
            
            spriteBatch.Draw(dataBackdrop, new Rectangle(5, 5, 205, 85), Color.White);
            
            spriteBatch.DrawString(spriteFont, $"{"Initialize:", -15}{initialize.ElapsedMilliseconds, 4} ms", new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(spriteFont, $"{"Update:", -15}{update.ElapsedMilliseconds, 4} ms", new Vector2(10, 30), Color.Black);
            spriteBatch.DrawString(spriteFont, $"{"Draw:", -15}{draw.ElapsedMilliseconds, 4} ms", new Vector2(10, 50), Color.Black);
            spriteBatch.DrawString(spriteFont, $"{"Total:", -15}{update.ElapsedMilliseconds + draw.ElapsedMilliseconds, 4} ms", new Vector2(10, 70), Color.Black);
            
            spriteBatch.End();
        }
    }
}
