using Common;
using ComponentGame.Components;
using Microsoft.Xna.Framework;
using MathF = System.MathF;

namespace ComponentGame.Systems
{
    public class VelocityModifierSystem
    {
        private readonly int length;
        private readonly int velocityModifierOffset;
        private readonly PositionComponent[] positionComponents;
        private readonly VelocityConstraintComponent[] velocityConstraintComponents;
        private readonly VelocityComponent[] velocityComponents;
        private readonly SizeComponent[] sizeComponents;
        private readonly VelocityModifierComponent[] modifierComponents;

        public VelocityModifierSystem(
            int length,
            int velocityModifierLength,
            PositionComponent[] positionComponents,
            VelocityConstraintComponent[] velocityConstraintComponents,
            VelocityComponent[] velocityComponents,
            SizeComponent[] sizeComponents,
            VelocityModifierComponent[] modifierComponents)
        {
            this.length = length;
            this.velocityModifierOffset = length - velocityModifierLength;
            this.positionComponents = positionComponents;
            this.velocityComponents = velocityComponents;
            this.sizeComponents = sizeComponents;
            this.modifierComponents = modifierComponents;
            this.velocityConstraintComponents = velocityConstraintComponents;
        }

        public void Update(float deltaTime)
        {
            for(int i = this.velocityModifierOffset; i < this.length; i++)
            {
                for(int j = 0; j < this.velocityModifierOffset; j++)
                {
                    var diffX = MathF.Abs(this.positionComponents[j].X - this.positionComponents[i].X);
                    var diffY = MathF.Abs(this.positionComponents[j].Y - this.positionComponents[i].Y);

                    var modifierIndex = i - this.velocityModifierOffset;
                    if(MathF.Pow(this.sizeComponents[modifierIndex].Value, 2) < MathF.Pow(diffX, 2) + MathF.Pow(diffY, 2))
                    {
                        continue;
                    }
                    
                    var velocityVec = new Vector2(this.velocityComponents[j].X, this.velocityComponents[j].Y);
                    velocityVec = velocityVec + velocityVec * this.modifierComponents[modifierIndex].Value * deltaTime;
                    velocityVec = velocityVec.ClampMagnitude(this.velocityConstraintComponents[j].Min, this.velocityConstraintComponents[j].Max);

                    var velocity = new VelocityComponent();
                    velocity.X = velocityVec.X;
                    velocity.Y = velocityVec.Y;
                    this.velocityComponents[j] = velocity;
                }
            }
        }
    }
}
