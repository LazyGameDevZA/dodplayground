using System;
using System.Collections.Generic;
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
        private Texture2D[] texture2Ds;

        public ObjectOrientedGame()
        {
            PerfMon.InitializeStarted();
            
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            this.gameObjects = new List<IGameObject>(DotCount + BubbleCount);
            
            var bubbles = new Bubble[BubbleCount];
            
            var random = new Random();

            for(int i = 0; i < DotCount; i++)
            {
                var dot = new Dot(random, bubbles);
                this.gameObjects.Add(dot);
            }

            for(int i = 0; i < BubbleCount; i++)
            {
                var bubble = new Bubble(random);
                this.gameObjects.Add(bubble);
                bubbles[i] = bubble;
            }
        }

        protected override void Initialize()
        {
            var displaySize = this.graphics.GraphicsDevice.DisplayMode.TitleSafeArea;
            this.graphics.PreferredBackBufferHeight = displaySize.Height - 100;
            this.graphics.PreferredBackBufferWidth = displaySize.Width;
            this.graphics.ApplyChanges();

            for(int i = 0; i < this.gameObjects.Count; i++)
            {
                this.gameObjects[i].Initialize(this.graphics);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var perfSpriteBatch = new SpriteBatch(this.GraphicsDevice);
            var font = this.Content.Load<SpriteFont>(nameof(Fonts.Consolas));

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.texture2Ds = new Texture2D[2];
            this.texture2Ds[Sprites.Dot] = this.Content.Load<Texture2D>(nameof(Sprites.Dot));
            this.texture2Ds[Sprites.Bubble] = this.Content.Load<Texture2D>(nameof(Sprites.Bubble));
            
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
                var texture2D = this.texture2Ds[this.gameObjects[i].SpriteIndex];
                var origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
                this.spriteBatch.Draw(texture2D, this.gameObjects[i].Position, null, this.gameObjects[i].SpriteColor, 0.0f, origin, Vector2.One,
                    SpriteEffects.None, 0.0f);
            }

            this.spriteBatch.End();

            base.Draw(gameTime);
            PerfMon.DrawFinished();
        }
    }
}
