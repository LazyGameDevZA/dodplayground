using System;
using Common;
using Common.PerformanceMonitoring;
using CompositionGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static CompositionGame.Scene;
using static Common.Constants;
using MathF = Common.MathF;

namespace CompositionGame
{
    public class CompositionGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly Texture2D[] texture2Ds;

        private SpriteBatch spriteBatch;

        public CompositionGame()
        {
            PerfMon.InitializeStarted();

            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
            
            this.texture2Ds = new Texture2D[2];
            var worldBoundsGameObject = new GameObject("World Bounds");
            var worldBoundsComponent = new WorldBoundsComponent(this.graphics);
            worldBoundsGameObject.AddComponent(worldBoundsComponent);
            s_GameObjects.Add(worldBoundsGameObject);

            var random = new Random();
            
            for(int i = 0; i < DotCount; i++)
            {
                var gameObject = new GameObject($"Dot #{i}");
                
                var positionComponent = new PositionComponent(random);
                gameObject.AddComponent(positionComponent);

                var velocityComponent = new VelocityComponent(random, Dot.MinVelocity, Dot.MaxVelocity);
                gameObject.AddComponent(velocityComponent);
                
                var spriteComponent = new SpriteComponent();
                var colors = new byte[3];
                random.NextBytes(colors);
                spriteComponent.ColorR = colors[0];
                spriteComponent.ColorG = colors[1];
                spriteComponent.ColorB = colors[2];
                spriteComponent.Alpha = byte.MaxValue;
                spriteComponent.Index = Sprites.Dot;
                gameObject.AddComponent(spriteComponent);
                
                var modifyVelocityComponent = new ModifyVelocityComponent();
                gameObject.AddComponent(modifyVelocityComponent);
                
                s_GameObjects.Add(gameObject);
            }

            for(var i = 0; i < BubbleCount; i++)
            {
                var gameObject = new GameObject($"Bubble #{i}");

                var positionComponent = new PositionComponent(random);
                gameObject.AddComponent(positionComponent);

                var velocityComponent = new VelocityComponent(random, Bubble.MinVelocity, Bubble.MaxVelocity);
                gameObject.AddComponent(velocityComponent);

                var velocityModifier = (float)random.NextDouble() * (Bubble.MaxModifier - Bubble.MinModifier) + Bubble.MinModifier;

                var spriteComponent = new SpriteComponent();
                spriteComponent.ColorR = MathB.Select(0, byte.MaxValue, velocityModifier < 0.0f);
                spriteComponent.ColorG = MathB.Select(0, byte.MaxValue, velocityModifier >= 0.0f);
                spriteComponent.ColorB = byte.MinValue;
                var scaleMax = MathF.Select(Bubble.MinModifier, Bubble.MaxModifier, velocityModifier >= 0.0f);
                spriteComponent.Alpha = (byte)(int)(128 * (velocityModifier / scaleMax));
                spriteComponent.Index = Sprites.Bubble;
                gameObject.AddComponent(spriteComponent);

                var velocityModifierComponent = new VelocityModifierComponent();
                velocityModifierComponent.VelocityModifier = velocityModifier;
                gameObject.AddComponent(velocityModifierComponent);
                
                var sizeComponent = new SizeComponent();
                sizeComponent.Size = 64;
                gameObject.AddComponent(sizeComponent);

                s_GameObjects.Add(gameObject);
            }
        }

        protected override void Initialize()
        {

            var displaySize = this.graphics.GraphicsDevice.DisplayMode.TitleSafeArea;
            this.graphics.PreferredBackBufferHeight = displaySize.Height - 100;
            this.graphics.PreferredBackBufferWidth = displaySize.Width;
            this.graphics.ApplyChanges();

            for(var i = 0; i < s_GameObjects.Count; i++)
            {
                s_GameObjects[i].Start();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var perfSpriteBatch = new SpriteBatch(this.GraphicsDevice);
            var font = this.Content.Load<SpriteFont>(nameof(Fonts.Consolas));

            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.texture2Ds[Sprites.Dot] = Content.Load<Texture2D>(nameof(Sprites.Dot));
            this.texture2Ds[Sprites.Bubble] = Content.Load<Texture2D>(nameof(Sprites.Bubble));

            PerfMon.InitializeFinished(perfSpriteBatch, font);
        }

        protected override void Update(GameTime gameTime)
        {
            PerfMon.UpdateStarted();
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach(var gameObject in s_GameObjects)
            {
                gameObject.Update(deltaTime);
            }

            base.Update(gameTime);
            PerfMon.UpdateFinished();
        }

        protected override void Draw(GameTime gameTime)
        {
            PerfMon.DrawStarted();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();
            foreach(var gameObject in s_GameObjects)
            {
                var position = gameObject.GetComponent<PositionComponent>();
                var sprite = gameObject.GetComponent<SpriteComponent>();

                if(position != null && sprite != null)
                {
                    var texture2D = this.texture2Ds[sprite.Index];
                    var color = new Color(sprite.ColorR, sprite.ColorG, sprite.ColorB, sprite.Alpha);
                    var origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
                    this.spriteBatch.Draw(texture2D, position.Value, null, color, 0.0f, origin, Vector2.One, SpriteEffects.None, 0.0f);
                }
            }
            this.spriteBatch.End();
            base.Draw(gameTime);
            PerfMon.DrawFinished();
        }
    }
}
