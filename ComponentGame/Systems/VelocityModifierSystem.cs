using System;
using Common;
using ComponentGame.Components;
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
            var positions = new Span<PositionComponent>(this.positionComponents);
            var velocityConstraints = new Span<VelocityConstraintComponent>(this.constraintComponents);
            var velocities = new Span<VelocityComponent>(this.velocityComponents);
            var modifiers = new Span<VelocityModifierComponent>(this.modifierComponents);
            
            for(int i = this.velocityModifierOffset; i < this.length; i++)
            {
                for(int j = 0; j < this.velocityModifierOffset; j++)
                {
                    var diff = positions[j].Value - positions[i].Value;

                    var modifierIndex = i - this.velocityModifierOffset;
                    if(MathF.Pow(this.sizeComponents[modifierIndex].Value, 2) < diff.LengthSquared())
                    {
                        continue;
                    }

                    var velocity = velocities[j];
                    var velocityValue = velocity.Value;
                    velocityValue = velocityValue + velocityValue * modifiers[modifierIndex].Value * deltaTime;
                    velocity.Value = velocityValue.ClampMagnitude(velocityConstraints[j].Min, velocityConstraints[j].Max);

                    velocities[j] = velocity;
                }
            }
        }
    }
}
