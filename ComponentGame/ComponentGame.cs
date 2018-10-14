using System;
using Common;
using Common.PerformanceMonitoring;
using ComponentGame.Components;
using ComponentGame.Systems;
using ComponentGame.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static Common.Constants;
using Color = Microsoft.Xna.Framework.Color;
using Math = Common.Math;
using MathF = Common.MathF;

namespace ComponentGame
{
    public class ComponentGame : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private readonly Texture2D[] texture2Ds;
        private readonly int entityCount;
        private readonly int velocityModifierCount;
        private readonly PositionComponent[] positionComponents;
        private readonly VelocityConstraintComponent[] velocityConstraintComponents;
        private readonly VelocityComponent[] velocityComponents;
        private readonly SpriteComponent[] spriteComponents;
        private readonly SizeComponent[] sizeComponents;
        private readonly VelocityModifierComponent[] velocityModifierComponents;

        private MoveSystem moveSystem;
        private VelocityModifierSystem velocityModifierSystem;
        private DrawSystem drawSystem;

        public ComponentGame()
        {
            PerfMon.InitializeStarted();
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            
            this.texture2Ds = new Texture2D[2];
            this.entityCount = DotCount + BubbleCount;
            this.positionComponents = new PositionComponent[this.entityCount];
            this.velocityConstraintComponents = new VelocityConstraintComponent[this.entityCount];
            this.velocityComponents = new VelocityComponent[this.entityCount];
            this.spriteComponents = new SpriteComponent[this.entityCount];

            this.velocityModifierCount = BubbleCount;
            this.sizeComponents = new SizeComponent[this.velocityModifierCount];
            this.velocityModifierComponents = new VelocityModifierComponent[this.velocityModifierCount];
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
            
            this.moveSystem = new MoveSystem(this.graphics, this.entityCount, this.positionComponents, this.velocityComponents);
            this.velocityModifierSystem = new VelocityModifierSystem(this.entityCount, this.velocityModifierCount, this.positionComponents,
                this.velocityConstraintComponents, this.velocityComponents, this.sizeComponents, this.velocityModifierComponents);

            var random = new Random();
            
            for(int i = 0; i < this.entityCount; i++)
            {
                var position = new PositionComponent();
                position.X = random.Next(minX, maxX);
                position.Y = random.Next(minY, maxY);
                this.positionComponents[i] = position;

                var entityTypePredicate = i >= DotCount;
                var velMin = MathF.Select(Dot.MinVelocity, Bubble.MinVelocity, entityTypePredicate);
                var velMax = MathF.Select(Dot.MaxVelocity, Bubble.MaxVelocity, entityTypePredicate);

                var velocityConstraint = new VelocityConstraintComponent();
                velocityConstraint.Min = velMin;
                velocityConstraint.Max = velMax;
                this.velocityConstraintComponents[i] = velocityConstraint;

                var velocity = new VelocityComponent();
                var velVector = random.NextVelocity(velMin, velMax);
                velocity.X = velVector.X;
                velocity.Y = velVector.Y;
                this.velocityComponents[i] = velocity;
                
                var velocityModifierValue = (float)random.NextDouble() * (Bubble.MaxModifier - Bubble.MinModifier) + Bubble.MinModifier;
                var scaleMax = MathF.Select(Bubble.MinModifier, Bubble.MaxModifier, velocityModifierValue >= 0.0f);
                var bubbleAlpha = (byte)(int)(128 * (velocityModifierValue / scaleMax));
                var bubbleColorR = MathB.Select(0, byte.MaxValue, velocityModifierValue < 0.0f);
                var bubbleColorG = MathB.Select(0, byte.MaxValue, velocityModifierValue >= 0.0f);

                var dotColors = new byte[3];
                random.NextBytes(dotColors);
                var sprite = new SpriteComponent();
                sprite.ColorR = MathB.Select(dotColors[0], bubbleColorR, entityTypePredicate);
                sprite.ColorG = MathB.Select(dotColors[1], bubbleColorG, entityTypePredicate);
                sprite.ColorB = MathB.Select(dotColors[2], byte.MinValue, entityTypePredicate);
                sprite.Alpha = MathB.Select(byte.MaxValue, bubbleAlpha, entityTypePredicate);
                sprite.Index = Math.Select(Sprites.Dot, Sprites.Bubble, entityTypePredicate);
                this.spriteComponents[i] = sprite;
                
                if(entityTypePredicate)
                {
                    var modifierIndex = i - DotCount;
                    
                    var size = new SizeComponent();
                    size.Value = 64;
                    this.sizeComponents[modifierIndex] = size;
                    
                    var velocityModifier = new VelocityModifierComponent();
                    velocityModifier.Value = velocityModifierValue;
                    this.velocityModifierComponents[modifierIndex] = velocityModifier;
                }
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var perfSpriteBatch = new SpriteBatch(this.GraphicsDevice);
            var font = this.Content.Load<SpriteFont>(nameof(Fonts.Consolas));

            var spriteBatch = new SpriteBatch(GraphicsDevice);

            this.texture2Ds[Sprites.Dot] = Content.Load<Texture2D>(nameof(Sprites.Dot));
            this.texture2Ds[Sprites.Bubble] = Content.Load<Texture2D>(nameof(Sprites.Bubble));
            
            this.drawSystem = new DrawSystem(spriteBatch, this.texture2Ds, this.entityCount, this.positionComponents, this.spriteComponents);

            PerfMon.InitializeFinished(perfSpriteBatch, font);
        }

        protected override void Update(GameTime gameTime)
        {
            PerfMon.UpdateStarted();
            
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.moveSystem.Update(deltaTime);
            this.velocityModifierSystem.Update(deltaTime);

            base.Update(gameTime);
            
            PerfMon.UpdateFinished();
        }

        protected override void Draw(GameTime gameTime)
        {
            PerfMon.DrawStarted();
            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.drawSystem.Draw();

            base.Draw(gameTime);
            
            PerfMon.DrawFinished();
        }
    }
}
