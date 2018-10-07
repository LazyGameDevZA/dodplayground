using System;
using System.Diagnostics;
using CompositionGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static CompositionGame.Scene;
using static Common.Constants;

namespace CompositionGame
{
    public class CompositionGame : Game
    {
        private readonly int dotCount;
        private readonly int bubbleCount;
        private readonly GraphicsDeviceManager graphics;
        private readonly Texture2D[] texture2Ds;
        
        SpriteBatch spriteBatch;

        public CompositionGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.texture2Ds = new Texture2D[2];
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
            
            var worldBoundsGameObject = new GameObject("World Bounds");
            var worldBoundsComponent = new WorldBoundsComponent(this.graphics);
            worldBoundsGameObject.AddComponent(worldBoundsComponent);
            s_GameObjects.Add(worldBoundsGameObject);

            var random = new Random();
            
            for(int i = 0; i < this.dotCount; i++)
            {
                var gameObject = new GameObject($"Dot #{i}");
                
                var positionComponent = new PositionComponent();
                positionComponent.X = random.Next(worldBoundsComponent.MinX, worldBoundsComponent.MaxX);
                positionComponent.Y = random.Next(worldBoundsComponent.MinY, worldBoundsComponent.MaxY);
                gameObject.AddComponent(positionComponent);

                var moveComponent = new MoveComponent(random, Dot.MinVelocity, Dot.MaxVelocity);
                gameObject.AddComponent(moveComponent);
                
                var spriteComponent = new SpriteComponent();
                var colors = new byte[3];
                random.NextBytes(colors);
                spriteComponent.ColorR = colors[0];
                spriteComponent.ColorG = colors[1];
                spriteComponent.ColorB = colors[2];
                spriteComponent.Alpha = byte.MaxValue;
                spriteComponent.index = Sprites.Dot;
                gameObject.AddComponent(spriteComponent);
                
                s_GameObjects.Add(gameObject);
                gameObject.Start();
            }

            base.Initialize();
            stopwatch.Stop();
            Console.WriteLine("Initialize completed after {0} milliseconds", stopwatch.ElapsedMilliseconds);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.texture2Ds[Sprites.Dot] = Content.Load<Texture2D>(nameof(Sprites.Dot));
            this.texture2Ds[Sprites.Bubble] = Content.Load<Texture2D>(nameof(Sprites.Bubble));
        }

        protected override void Update(GameTime gameTime)
        {
            var stopwatch = Stopwatch.StartNew();
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach(var gameObject in s_GameObjects)
            {
                gameObject.Update(deltaTime);
            }

            base.Update(gameTime);
            stopwatch.Stop();
            Console.WriteLine("Update completed after {0} milliseconds", stopwatch.ElapsedMilliseconds);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            foreach(var gameObject in s_GameObjects)
            {
                var pos = gameObject.GetComponent<PositionComponent>();
                var sprite = gameObject.GetComponent<SpriteComponent>();

                if(pos != null && sprite != null)
                {
                    var texture2D = this.texture2Ds[sprite.index];
                    var position = new Vector2(pos.X, pos.Y);
                    var color = new Color(sprite.ColorR, sprite.ColorG, sprite.ColorB, sprite.Alpha);
                    var origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
                    this.spriteBatch.Draw(texture2D, position, null, color, 0.0f, origin, Vector2.One, SpriteEffects.None, 0.0f);
                }
            }
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
