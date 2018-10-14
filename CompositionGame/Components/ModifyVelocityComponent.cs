using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using static CompositionGame.Utilities.ComponentUtilities;

namespace CompositionGame.Components
{
    internal class ModifyVelocityComponent : Component
    {
        private PositionComponent position;
        private VelocityComponent velocity;
        private static IList<VelocityModifierComponent> velocityModifiers;
        private static IList<PositionComponent> modifierPositions;
        private static IList<SizeComponent> modifierSizes;

        public override void Start()
        {
            this.position = this.GameObject.GetComponent<PositionComponent>();
            this.velocity = this.GameObject.GetComponent<VelocityComponent>();

            if(velocityModifiers != null && velocityModifiers.Count != 0)
            {
                return;
            }
            
            velocityModifiers = FindAllComponentsOfType<VelocityModifierComponent>();
            modifierPositions = new List<PositionComponent>(velocityModifiers.Count);
            modifierSizes = new List<SizeComponent>(velocityModifiers.Count);

            for(var i = 0; i < velocityModifiers.Count; i++)
            {
                modifierPositions.Add(velocityModifiers[i].GameObject.GetComponent<PositionComponent>());
                modifierSizes.Add(velocityModifiers[i].GameObject.GetComponent<SizeComponent>());
            }
        }

        public override void Update(float deltaTime)
        {
            for(var i = 0; i < modifierPositions.Count; i++)
            {
                var diffX = MathF.Abs(this.position.X - modifierPositions[i].X);
                var diffY = MathF.Abs(this.position.Y - modifierPositions[i].Y);

                if(MathF.Pow(modifierSizes[i].Size, 2) < MathF.Pow(diffX, 2) + MathF.Pow(diffY, 2))
                {
                    continue;
                }

                var velocity = new Vector2(this.velocity.X, this.velocity.Y);
                velocity = velocity + velocity * (velocityModifiers[i].VelocityModifier) * deltaTime;

                this.velocity.SetVelocity(velocity.X, velocity.Y);
            }
        }
    }
}
