using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common.PerformanceMonitoring;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ObjectOriented;
using static Common.Constants;

namespace OOGame
{
    public class ObjectOrientedGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly IList<IGameObject> gameObjects;

        private SpriteBatch spriteBatch;
        private Texture2D dot;
        private Texture2D bubble;

        public ObjectOrientedGame()
        {
            PerfMon.InitializeStarted();
            
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            this.gameObjects = new List<IGameObject>(DotCount + BubbleCount);
        }

        protected override void Initialize()
        {
            var displaySize = this.graphics.GraphicsDevice.DisplayMode.TitleSafeArea;
            this.graphics.PreferredBackBufferHeight = displaySize.Height - 100;
            this.graphics.PreferredBackBufferWidth = displaySize.Width;
            this.graphics.ApplyChanges();
            
            var bubbles = new Bubble[BubbleCount];
            
            var random = new Random();

            for(int i = 0; i < DotCount; i++)
            {
                var dot = new Dot(random, bubbles);
                dot.Initialize(graphics);
                this.gameObjects.Add(dot);
            }

            for(int i = 0; i < BubbleCount; i++)
            {
                var bubble = new Bubble(random);
                bubble.Initialize(this.graphics);
                this.gameObjects.Add(bubble);
                bubbles[i] = bubble;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var perfSpriteBatch = new SpriteBatch(this.GraphicsDevice);
            var font = this.Content.Load<SpriteFont>(nameof(Fonts.Consolas));

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.dot = this.Content.Load<Texture2D>(nameof(Sprites.Dot));
            this.bubble = this.Content.Load<Texture2D>(nameof(Sprites.Bubble));

            for(var i = 0; i < this.gameObjects.Count; i++)
            {
                if(this.gameObjects[i] is Dot)
                {
                    this.gameObjects[i].LoadContent(this.spriteBatch, this.dot);
                    continue;
                }

                if(this.gameObjects[i] is Bubble)
                {
                    this.gameObjects[i].LoadContent(this.spriteBatch, this.bubble);
                    continue;
                }
            }
            
            PerfMon.InitializeFinished(perfSpriteBatch, font);
        }

        protected override void Update(GameTime gameTime)
        {
            PerfMon.UpdateStarted();
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            for(var i = 0; i < this.gameObjects.Count; i++)
            {
                this.gameObjects[i].Update(gameTime);
            }

            base.Update(gameTime);
            PerfMon.UpdateFinished();
        }

        protected override void Draw(GameTime gameTime)
        {
            PerfMon.DrawStarted();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            for(var i = 0; i < this.gameObjects.Count; i++)
            {
                this.gameObjects[i].Draw(gameTime);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
            PerfMon.DrawFinished();
        }
    }
}
