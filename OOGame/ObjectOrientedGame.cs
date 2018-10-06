using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OOGame
{
    public class ObjectOrientedGame : Game
    {
        private readonly int dotCount;
        private readonly int bubbleCount;
        private readonly GraphicsDeviceManager graphics;

        private Dot[] dots;
        private Bubble[] bubbles;

        private SpriteBatch spriteBatch;
        private Texture2D dot;
        private Texture2D bubble;

        public ObjectOrientedGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;

            this.dotCount = 1000000;
            this.bubbleCount = 20;
        }

        protected override void Initialize()
        {
            var stopwatch = Stopwatch.StartNew();
            var displaySize = this.graphics.GraphicsDevice.DisplayMode.TitleSafeArea;
            this.graphics.PreferredBackBufferHeight = displaySize.Height - 100;
            this.graphics.PreferredBackBufferWidth = displaySize.Width;
            this.graphics.ApplyChanges();
            
            this.dots = new Dot[this.dotCount];
            this.bubbles = new Bubble[this.bubbleCount];
            
            var random = new Random();

            for(int i = 0; i < this.dotCount; i++)
            {
                this.dots[i] = new Dot(random, this.bubbles);
                this.dots[i].Initialize(graphics);
            }

            for(int i = 0; i < this.bubbleCount; i++)
            {
                this.bubbles[i] = new Bubble(random);
                this.bubbles[i].Initialize(this.graphics);
            }

            base.Initialize();
            stopwatch.Stop();
            Console.WriteLine("Initialize completed after {0} milliseconds", stopwatch.ElapsedMilliseconds);
        }

        protected override void LoadContent()
        {
            var stopwatch = Stopwatch.StartNew();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.dot = Content.Load<Texture2D>("Dot");
            this.bubble = Content.Load<Texture2D>("Bubble");
            
            for(int i = 0; i < this.dots.Length; i++)
            {
                this.dots[i].LoadContent(spriteBatch, this.dot);
            }

            for(int i = 0; i < this.bubbles.Length; i++)
            {
                this.bubbles[i].LoadContent(this.spriteBatch, this.bubble);
            }
            stopwatch.Stop();
            Console.WriteLine("Load Content completed after {0} milliseconds", stopwatch.ElapsedMilliseconds);
        }

        protected override void Update(GameTime gameTime)
        {
            var stopwatch = Stopwatch.StartNew();
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for(int i = 0; i < this.dots.Length; i++)
            {
                this.dots[i].Update(gameTime);
            }

            for(int i = 0; i < this.bubbles.Length; i++)
            {
                this.bubbles[i].Update(gameTime);
            }

            base.Update(gameTime);
            stopwatch.Stop();
            Console.WriteLine("Update completed after {0} milliseconds", stopwatch.ElapsedMilliseconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            var stopwatch = Stopwatch.StartNew();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            
            for(var i = 0; i < this.dots.Length; i++)
            {
                this.dots[i].Draw(gameTime);
            }

            for(var i = 0; i < this.bubbles.Length; i++)
            {
                this.bubbles[i].Draw(gameTime);
            }
            
            this.spriteBatch.End();

            base.Draw(gameTime);
            stopwatch.Stop();
            Console.WriteLine("Draw completed after {0} milliseconds", stopwatch.ElapsedMilliseconds);
        }
    }
}
