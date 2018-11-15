using System;
using System.Threading.Tasks;
using Common;
using Common.PerformanceMonitoring;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiThreadComponentGame.Components;
using MultiThreadComponentGame.Systems;
using MultiThreadComponentGame.Utilities;
using Color = Microsoft.Xna.Framework.Color;
using Math = Common.Math;
using MathF = Common.MathF;

namespace MultiThreadComponentGame
{
    public class MultiThreadComponentGame : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private MoveSystem moveSystem;
        private VelocityModifierSystem velocityModifierSystem;

        private Texture2D[] texture2Ds;
        private SpriteBatch spriteBatch;

        public MultiThreadComponentGame()
        {
            PerfMon.InitializeStarted();
            
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            this.IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            var displaySize = this.graphics.GraphicsDevice.DisplayMode.TitleSafeArea;
            this.graphics.PreferredBackBufferHeight = displaySize.Height - 100;
            this.graphics.PreferredBackBufferWidth = displaySize.Width;
            this.graphics.ApplyChanges();

            const int minX = 0;
            var maxX = this.graphics.PreferredBackBufferWidth;
            const int minY = 0;
            var maxY = this.graphics.PreferredBackBufferHeight;
            
            this.moveSystem = new MoveSystem(this.graphics, Scene.EntityCount, Scene.PositionComponents, Scene.VelocityComponents);
            this.velocityModifierSystem = new VelocityModifierSystem(Scene.EntityCount, Scene.VelocityModifierCount, Scene.PositionComponents,
                Scene.VelocityConstraintComponents, Scene.VelocityComponents, Scene.SizeComponents, Scene.VelocityModifierComponents);

            var random = new Random();
            
            for(int i = 0; i < Scene.EntityCount; i++)
            {
                var position = new PositionComponent();
                position.Value = new Vector2(random.Next(minX, maxX), random.Next(minY, maxY));
                Scene.PositionComponents[i] = position;

                var entityTypePredicate = i >= Constants.DotCount;
                var velMin = MathF.Select(Constants.Dot.MinVelocity, Constants.Bubble.MinVelocity, entityTypePredicate);
                var velMax = MathF.Select(Constants.Dot.MaxVelocity, Constants.Bubble.MaxVelocity, entityTypePredicate);

                Scene.VelocityConstraintComponents[i] = new VelocityConstraintComponent(velMin, velMax);

                var velocity = new VelocityComponent();
                velocity.Value = random.NextVelocity(velMin, velMax);
                Scene.VelocityComponents[i] = velocity;
                
                var velocityModifierValue = (float)random.NextDouble() * (Constants.Bubble.MaxModifier - Constants.Bubble.MinModifier) + Constants.Bubble.MinModifier;
                var scaleMax = MathF.Select(Constants.Bubble.MinModifier, Constants.Bubble.MaxModifier, velocityModifierValue >= 0.0f);
                var bubbleAlpha = (byte)(int)(128 * (velocityModifierValue / scaleMax));
                var bubbleColorR = MathB.Select(0, byte.MaxValue, velocityModifierValue < 0.0f);
                var bubbleColorG = MathB.Select(0, byte.MaxValue, velocityModifierValue >= 0.0f);

                var dotColors = new byte[3];
                random.NextBytes(dotColors);
                var colorR = MathB.Select(dotColors[0], bubbleColorR, entityTypePredicate);
                var colorG = MathB.Select(dotColors[1], bubbleColorG, entityTypePredicate);
                var colorB = MathB.Select(dotColors[2], byte.MinValue, entityTypePredicate);
                var alpha = MathB.Select(byte.MaxValue, bubbleAlpha, entityTypePredicate);
                var index = Math.Select(Constants.Sprites.Dot, Constants.Sprites.Bubble, entityTypePredicate);
                Scene.SpriteComponents[i] = new SpriteComponent(colorR, colorG, colorB, alpha, index);
                
                if(entityTypePredicate)
                {
                    var modifierIndex = i - Constants.DotCount;
                    
                    Scene.SizeComponents[modifierIndex] = new SizeComponent(64);
                    
                    Scene.VelocityModifierComponents[modifierIndex] = new VelocityModifierComponent(velocityModifierValue);
                }
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var perfSpriteBatch = new SpriteBatch(this.GraphicsDevice);
            var font = this.Content.Load<SpriteFont>(nameof(Constants.Fonts.Consolas));

            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.texture2Ds = new Texture2D[2];
            this.texture2Ds[Constants.Sprites.Dot] = this.Content.Load<Texture2D>(nameof(Constants.Sprites.Dot));
            this.texture2Ds[Constants.Sprites.Bubble] = this.Content.Load<Texture2D>(nameof(Constants.Sprites.Bubble));
            
            PerfMon.InitializeFinished(perfSpriteBatch, font);
        }

        protected override void Update(GameTime gameTime)
        {
            PerfMon.UpdateStarted();
            
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var moveJobs = this.moveSystem.Update(deltaTime, Task.CompletedTask);
            var velocityModifierJobs = this.velocityModifierSystem.Update(deltaTime, moveJobs);
            
            velocityModifierJobs.Wait();

            base.Update(gameTime);
            
            PerfMon.UpdateFinished();
        }

        protected override void Draw(GameTime gameTime)
        {
            PerfMon.DrawStarted();
            
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin(SpriteSortMode.FrontToBack);

            var sprites = new Span<SpriteComponent>(Scene.SpriteComponents);
            var positions = new Span<PositionComponent>(Scene.PositionComponents);

            for(int i = 0; i < Scene.EntityCount; i++)
            {
                var texture2D = this.texture2Ds[sprites[i].Index];
                var color = new Color(sprites[i].ColorR, sprites[i].ColorG, sprites[i].ColorB, sprites[i].Alpha);
                var origin = new Vector2(texture2D.Width / 2, texture2D.Height / 2);
                this.spriteBatch.Draw(texture2D, positions[i].Value, null, color, 0.0f, origin, Vector2.One, SpriteEffects.None, sprites[i].Index);
            }
            
            this.spriteBatch.End();

            base.Draw(gameTime);
            
            PerfMon.DrawFinished();
        }
    }
}
