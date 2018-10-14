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
        private readonly VelocityConstraintComponent[] constraintComponents;
        private readonly VelocityComponent[] velocityComponents;
        private readonly SizeComponent[] sizeComponents;
        private readonly VelocityModifierComponent[] modifierComponents;

        public VelocityModifierSystem(
            int length,
            int velocityModifierLength,
            PositionComponent[] positionComponents,
            VelocityConstraintComponent[] constraintComponents,
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
            this.constraintComponents = constraintComponents;
        }

        public void Update(float deltaTime)
        {
            for(int i = this.velocityModifierOffset; i < this.length; i++)
            {
                for(int j = 0; j < this.velocityModifierOffset; j++)
                {
                    var diff = this.positionComponents[j].Value - this.positionComponents[i].Value;

                    var modifierIndex = i - this.velocityModifierOffset;
                    if(MathF.Pow(this.sizeComponents[modifierIndex].Value, 2) < diff.LengthSquared())
                    {
                        continue;
                    }

                    var velocity = this.velocityComponents[j];
                    var velocityValue = velocity.Value;
                    velocityValue = velocityValue + velocityValue * this.modifierComponents[modifierIndex].Value * deltaTime;
                    velocity.Value = velocityValue.ClampMagnitude(this.constraintComponents[j].Min, this.constraintComponents[j].Max);

                    this.velocityComponents[j] = velocity;
                }
            }
        }
    }
}
