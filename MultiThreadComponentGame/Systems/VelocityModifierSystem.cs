using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using MultiThreadComponentGame.Components;
using MultiThreadComponentGame.Utilities;
using Math = System.Math;
using MathF = System.MathF;

namespace MultiThreadComponentGame.Systems
{
    public class VelocityModifierSystem
    {
        private readonly int modifierLength;
        private readonly int length;
        private readonly int sliceLength;
        private readonly PositionComponent[] positionComponents;
        private readonly VelocityConstraintComponent[] constraintComponents;
        private readonly VelocityComponent[] velocityComponents;
        private readonly SizeComponent[] sizeComponents;
        private readonly VelocityModifierComponent[] modifierComponents;

        public VelocityModifierSystem(
            int length,
            int modifierLength,
            PositionComponent[] positionComponents,
            VelocityConstraintComponent[] constraintComponents,
            VelocityComponent[] velocityComponents,
            SizeComponent[] sizeComponents,
            VelocityModifierComponent[] modifierComponents)
        {
            this.modifierLength = modifierLength;
            this.length = length - modifierLength;
            this.sliceLength = length / Environment.ProcessorCount + 1;
            this.positionComponents = positionComponents;
            this.velocityComponents = velocityComponents;
            this.sizeComponents = sizeComponents;
            this.modifierComponents = modifierComponents;
            this.constraintComponents = constraintComponents;
        }

        public Task Update(float deltaTime, Task inputDeps)
        {
            var tasks = this.ScheduleJobs(deltaTime, inputDeps);

            return Task.WhenAll(tasks);
        }

        private IEnumerable<Task> ScheduleJobs(float deltaTime, Task inputDeps)
        {
            ReadOnlyMemory<PositionComponent> allPositions = this.positionComponents;
            
            //Velocity to be modified
            ReadOnlyMemory<PositionComponent> positions = allPositions.Slice(0, this.length);
            ReadOnlyMemory<VelocityConstraintComponent> velocityConstraints = this.constraintComponents.Slice(0, this.length);
            Memory<VelocityComponent> velocities = this.velocityComponents.Slice(0, this.length);
            
            //Velocity modifier data
            ReadOnlyMemory<VelocityModifierComponent> modifiers = this.modifierComponents;
            ReadOnlyMemory<PositionComponent> modifierPositions = allPositions.Slice(this.length);
            ReadOnlyMemory<SizeComponent> modifierSizes = this.sizeComponents;

            for(int i = 0; i < Environment.ProcessorCount; i++)
            {
                var startIndex = i * this.sliceLength;
                var sliceLength = Math.Min(this.sliceLength, this.length - startIndex);

                var positionsSlice = positions.Slice(startIndex, sliceLength);
                var velocityConstraintsSlice = velocityConstraints.Slice(startIndex, sliceLength);
                var velocitiesSlice = velocities.Slice(startIndex, sliceLength);
                yield return inputDeps.ContinueWith((x) => RunJob(
                                                        deltaTime,
                                                        sliceLength,
                                                        this.modifierLength,
                                                        positionsSlice,
                                                        velocityConstraintsSlice,
                                                        velocitiesSlice,
                                                        modifiers,
                                                        modifierPositions,
                                                        modifierSizes));
            }
        }

        private static void RunJob(
            float deltaTime,
            int sliceLength,
            int modifierLength,
            ReadOnlyMemory<PositionComponent> positionsSlice,
            ReadOnlyMemory<VelocityConstraintComponent> velocityConstraintsSlice,
            Memory<VelocityComponent> velocitiesSlice,
            ReadOnlyMemory<VelocityModifierComponent> allModifiers,
            ReadOnlyMemory<PositionComponent> allModifierPositions,
            ReadOnlyMemory<SizeComponent> allModifierSizes)
        {
            //Velocity to be modified
            var positions = positionsSlice.Span;
            var velocityConstraints = velocityConstraintsSlice.Span;
            var velocities = velocitiesSlice.Span;
            
            //Velocity modifier data
            var modifiers = allModifiers.Span;
            var modifierPositions = allModifierPositions.Span;
            var sizes = allModifierSizes.Span;

            for(int i = 0; i < modifierLength; i++)
            {
                for(int j = 0; j < sliceLength; j++)
                {
                    var diff = positions[j].Value - modifierPositions[i].Value;

                    if(MathF.Pow(sizes[i].Value, 2) < diff.LengthSquared())
                    {
                        continue;
                    }

                    var velocity = velocities[j];
                    var velocityValue = velocity.Value;
                    velocityValue += velocityValue * modifiers[i].Value * deltaTime;
                    velocity.Value = velocityValue.ClampMagnitude(velocityConstraints[j].Min, velocityConstraints[j].Max);

                    velocities[j] = velocity;
                }
            }
        }
    }
}
