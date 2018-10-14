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
                var diff = this.position.Value - modifierPositions[i].Value;

                if(MathF.Pow(modifierSizes[i].Size, 2) < diff.LengthSquared())
                {
                    continue;
                }

                var velocity = this.velocity.Value + this.velocity.Value * (velocityModifiers[i].VelocityModifier) * deltaTime;

                this.velocity.SetVelocity(velocity);
            }
        }
    }
}
